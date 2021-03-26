

//调用示例
//let CompressRes = imageCompress(file,
//  { maxSize: 3 * 1024, maxWidth: 550, maxHeight: 550 }
//).then(res => {
//  console.log(res);
//})

//压缩大小和长宽的方法
//https://blog.csdn.net/qq_20567691/article/details/100044142
//eg. img:img= new Image() 
//设置最大长宽限制
const setRelativeMaxMin = (img, maxWidth, maxHeight) => {
    // 图片原始尺寸
    let originWidth = img.width;
    let originHeight = img.height;

    if (!maxWidth) { maxWidth = originWidth }
    if (!maxHeight) { maxHeight = originHeight }
 
    // 目标尺寸
    let targetWidth = originWidth, targetHeight = originHeight;
    // 图片尺寸超过400x400的限制
    if (originWidth > maxWidth || originHeight > maxHeight) {
      if (originWidth / originHeight > maxWidth / maxHeight) {
        // 更宽，按照宽度限定尺寸
        targetWidth = maxWidth;
        targetHeight = Math.round(maxWidth * (originHeight / originWidth));
      } else {
        targetHeight = maxHeight;
        targetWidth = Math.round(maxHeight * (originWidth / originHeight));
      }
    }

    return { targetWidth, targetHeight}
};

// 将File（Blob）对象转变为一个dataURL字符串， 即base64格式
const fileToDataURL = file => new Promise((resolve) => {
  const reader = new FileReader();
  reader.onloadend = e => resolve(e.target.result);
  reader.readAsDataURL(file);
});

// 将dataURL字符串转变为image对象，即base64转img对象
const dataURLToImage = dataURL => new Promise((resolve) => {
  const img = new Image();
  img.onload = () => resolve(img);
  img.src = dataURL;
});

// 将一个canvas对象转变为一个File（Blob）对象
const canvastoFile = (canvas, type, quality) => new Promise(resolve => canvas.toBlob(blob => resolve(blob), type, quality));

//eg. originfile: input.files[0]
//eg. maxSize: 1024 KB
//eg. maxSize: maxWidth、maxHeight、absoluteWidth、absoluteHeight 像素
//eg. scale: 比例 0-1之间
//config={maxSize, maxWidth, maxHeight, absoluteWidth, absoluteHeight, scale}
const imageCompress = (originfile, config) => new Promise(async (resolve, reject) => {
  const originSize = originfile.size / 1024; // 单位为kb
  if (!config.maxSize) { config.maxSize = originSize }

  console.log('图片指定最大尺寸为', config.maxSize, '原始尺寸为：', originSize);

  // 将原图片转换成base64
  const base64 = await fileToDataURL(originfile);
  console.log("将原图片转换成base64")
  
  // 缩放图片需要的canvas
  const canvas = document.createElement('canvas');
  const context = canvas.getContext('2d');

  // 小于maxSize，则不需要压缩，直接返回
  //if (originSize <= config.maxSize) {
    //resolve({ compressBase64: base64, compressFile: originfile });
    //console.log(`图片小于指定大小:${config.maxSize}KB，不用压缩`);
    //return;
  //}
  const img = await dataURLToImage(base64);
  let scale = 1;
  const originWidth = img.width;
  const originHeight = img.height;
 
  
  let maxWHSize = setRelativeMaxMin(img, config.maxWidth, config.maxHeight)
  let targetWidth = maxWHSize.targetWidth
  let targetHeight = maxWHSize.targetHeight


  if (config.absoluteWidth) {
    targetWidth = config.absoluteWidth
  }
  if (config.absoluteHeight) {
    targetHeight = config.absoluteHeight
  }

  if (config.scale) {
    if (0 < config.scale && config.scale < 1) {
      scale = config.scale
    }
    else {
      scale=1
    }
    targetWidth = originWidth * scale;
    targetHeight = originHeight * scale;
  }

  console.log("originWidth：" + originWidth)
  console.log("originHeight：" + originHeight)
  console.log("targetWidth：" + targetWidth)
  console.log("targetHeight：" + targetHeight)


  canvas.width = targetWidth;
  canvas.height = targetHeight;
  context.clearRect(0, 0, targetWidth, targetHeight);
  context.drawImage(img, 0, 0, targetWidth, targetHeight);

  // 将Canvas对象转变为dataURL字符串，即压缩后图片的base64格式
  // const compressedBase64 = canvas.toDataURL('image/jpeg', 0.1);本例子中未使用此方法压缩
  // 经过我的对比，通过scale控制图片的拉伸来压缩图片，能够压缩jpg，png等格式的图片
  // 通过canvastoFile方法传递quality来压缩图片，只能压缩jpeg类型的图片，png等格式不支持
  // scale的压缩效果没有canvastoFile好
  // 在压缩到指定大小时，通过scale压缩的图片比通过quality压缩的图片模糊的多
  // 压缩的思路，用二分法找最佳的压缩点
  // 这里为了规避浮点数计算的弊端，将quality转为整数再计算;
  // const preQuality = 100;
  const maxQualitySize = { quality: 100, size: Number.MAX_SAFE_INTEGER };
  const minQualitySize = { quality: 0, size: 0 };
  let quality = 100;
  let count = 0; // 压缩次数
  let compressFinish = false; // 压缩完成
  let invalidDesc = '';
  let compressBlob = null;

  // 二分法最多尝试8次即可覆盖全部可能
  while (!compressFinish && count < 12) {
    compressBlob = await canvastoFile(canvas, 'image/jpeg', quality / 100);
    const compressSize = compressBlob.size / 1024;
    count++;
    console.log("compressSize：" + compressSize)
    console.log("config.maxSize：" + config.maxSize)
    if (compressSize === config.maxSize) {
    //if (compressSize <= config.maxSize) {
      console.log(`压缩完成，总共压缩了${count}次`);
      compressFinish = true;
      return;
    }
    
    if (compressSize > config.maxSize) {
      maxQualitySize.quality = quality;
      maxQualitySize.size = compressSize;
    }
    if (compressSize < config.maxSize) {
      minQualitySize.quality = quality;
      minQualitySize.size = compressSize;
    }
    console.log(`第${count}次压缩,压缩后大小${compressSize},quality参数:${quality}`);

    quality = Math.ceil((maxQualitySize.quality + minQualitySize.quality) / 2);

    if (maxQualitySize.quality - minQualitySize.quality < 2) {
      if (!minQualitySize.size && quality) {
        quality = minQualitySize.quality;
      } else if (!minQualitySize.size && !quality) {
        compressFinish = true;
        invalidDesc = '压缩失败，无法压缩到指定大小';
        console.log(`压缩完成，总共压缩了${count}次`);
      } else if (minQualitySize.size > config.maxSize) {
        compressFinish = true;
        invalidDesc = '压缩失败，无法压缩到指定大小';
        console.log(`压缩完成，总共压缩了${count}次`);
      } else {
        console.log(`压缩完成，总共压缩了${count}次`);
        compressFinish = true;
        quality = minQualitySize.quality;
      }
    }
  }

  if (invalidDesc) {
    // 压缩失败，则返回原始图片的信息
    console.log(`压缩失败，无法压缩到指定大小：${config.maxSize}KB`);
    reject({ msg: invalidDesc, compressBase64: base64, compressFile: originfile });
    return;
  }

  //compressBlob = await canvastoFile(canvas, 'image/jpeg', quality / 100);
  //const compressSize = compressBlob.size / 1024;
  //console.log(`最后一次压缩（即第${count + 1}次），quality为:${quality}，大小：${compressSize}`);
  const compressedBase64 = await fileToDataURL(compressBlob);

  const compressedFile = new File([compressBlob], originfile.name, { type: 'image/jpeg' });

  return resolve({ compressFile: compressedFile, compressBase64: compressedBase64, compressBlob: compressBlob, width: targetWidth, height: targetHeight });
});


export default imageCompress;

//nom image-conversion上传图片
//import * as imageConversion from 'image-conversion';//图片压缩
//return new Promise((resolve, reject) => {
//  imageConversion.compressAccurately(file, {
//    size: 1024,    //The compressed image size is 100kb
//    accuracy: 1,//the accuracy of image compression size,range 0.8-0.99,default 0.95;
//    //this means if the picture size is set to 1000Kb and the
//    //accuracy is 0.9, the image with the compression result
//    //of 900Kb-1100Kb is considered acceptable;
//    quality: 1,	//optional	range 0-1, indicate the image quality, default 0.92
//    type: "image/jpeg",//如果压缩PNG透明图像，请选择“Image/png”类型   determine the converted image type; the options are "image/png", "image/jpeg", "image/gif",default "image/jpeg"
//    //width: 300,   //width
//    //height: 300,  //height
//    //orientation:1,//image rotation direction  1:0° 2:水平翻转 3、180° 4、垂直翻转 5、顺时针90°+水平翻转 6、顺时针90°7、顺时针90°+垂直翻转 8、逆时针90°
//    scale: 0.2   //the zoom ratio relative to the original image, range 0-10;
//    //Setting config.scale will override the settings of
//    //config.width and config.height;
//  }).then(res => {
//    let formData = new FormData();
//    formData.append("uploadFile", res, file.name);
//    serverApi.uploadFile(formData).then(response => {
//      _self.handleUploadSuccess(response, file)
//      console.log(response.Message[0].Path);
//      console.log(response);
//    })

//    //The res in the promise is a compressed Blob type (which can be treated as a File type) file;
//    return false
//  })
//})
