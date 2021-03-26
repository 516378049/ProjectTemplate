
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

const compress = (originfile, maxSize) => new Promise(async (resolve, reject) => {
  const originSize = originfile.size / 1024; // 单位为kb
  console.log('图片指定最大尺寸为', maxSize, '原始尺寸为：', originSize);

  // 将原图片转换成base64
  const base64 = await fileToDataURL(originfile);

  // 缩放图片需要的canvas
  const canvas = document.createElement('canvas');
  const context = canvas.getContext('2d');

  // 小于maxSize，则不需要压缩，直接返回
  if (originSize < maxSize) {
    resolve({ compressBase64: base64, compressFile: originfile });
    console.log(`图片小于指定大小:${maxSize}KB，不用压缩`);
    return;
  }


  const img = await dataURLToImage(base64);

  const scale = 1;
  const originWidth = img.width;
  const originHeight = img.height;
  const targetWidth = originWidth * scale;
  const targetHeight = originHeight * scale;

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
    if (compressSize === maxSize) {
      console.log(`压缩完成，总共压缩了${count}次`);
      compressFinish = true;
      return;
    }
    if (compressSize > maxSize) {
      maxQualitySize.quality = quality;
      maxQualitySize.size = compressSize;
    }
    if (compressSize < maxSize) {
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
      } else if (minQualitySize.size > maxSize) {
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
    console.log(`压缩失败，无法压缩到指定大小：${maxSize}KB`);
    reject({ msg: invalidDesc, compressBase64: base64, compressFile: originfile });
    return;
  }

  compressBlob = await canvastoFile(canvas, 'image/jpeg', quality / 100);
  const compressSize = compressBlob.size / 1024;
  console.log(`最后一次压缩（即第${count + 1}次），quality为:${quality}，大小：${compressSize}`);
  const compressedBase64 = await fileToDataURL(compressBlob);

  const compressedFile = new File([compressBlob], originfile.name, { type: 'image/jpeg' });

  resolve({ compressFile: compressedFile, compressBase64: compressedBase64 });
});


export default compress;









//压缩大小和长宽的方法
//https://blog.csdn.net/qq_20567691/article/details/100044142

//function changeImg(e) {
//  const imgFile = e.target.files[0];
//}


//const zipImgPromise = (imgFile, configs) => {
//  if (!configs.maxWidth) {
//    configs.maxWidth = 800
//  }
//  if (!configs.maxHeight) {
//    configs.maxHeight = 800;
//  }
//  return new Promise((resolve, reject) => {
//    let reader = new FileReader();    // 定义一个fileReader
//    reader.readAsDataURL(imgFile);    // 将图片转换成base64后可以得到图片的宽高
//    let img = new Image();
//    reader.onload = () => {
//      img.src = reader.result;
//    }
//    img.onload = () => {
//      // 图片原始尺寸
//      let originWidth = img.width;
//      let originHeight = img.height;
//      let canvas = document.createElement('canvas');
//      let context = canvas.getContext('2d');
//      // 最大尺寸限制
//      let maxWidth = configs.maxWidth, maxHeight = configs.maxHeight;
//      // 目标尺寸
//      let targetWidth = originWidth, targetHeight = originHeight;
//      // 图片尺寸超过400x400的限制
//      if (originWidth > maxWidth || originHeight > maxHeight) {
//        if (originWidth / originHeight > maxWidth / maxHeight) {
//          // 更宽，按照宽度限定尺寸
//          targetWidth = maxWidth;
//          targetHeight = Math.round(maxWidth * (originHeight / originWidth));
//        } else {
//          targetHeight = maxHeight;
//          targetWidth = Math.round(maxHeight * (originWidth / originHeight));
//        }
//      }
//      // canvas对图片进行缩放
//      canvas.width = targetWidth;
//      canvas.height = targetHeight;
//      // 清除画布
//      context.clearRect(0, 0, targetWidth, targetHeight);
//      // 图片压缩
//      context.drawImage(img, 0, 0, targetWidth, targetHeight);

//      // canvas转换成dataUrl
//      let dataUrl = canvas.toDataURL('image/png');
//      // 转换成formdata格式用于上传图片
//      let blob = this.dataURItoBlob(dataUrl);

//      if (blob) {
//        resolve(blob);
//      } else {
//        reject(new Error('Error！！'));
//      }
//    }
//  });
//};

//function dataURItoBlob(dataURI) {
//  let byteString = atob(dataURI.split(',')[1]);

//  let mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]

//  let ab = new ArrayBuffer(byteString.length);
//  let ia = new Uint8Array(ab);
//  for (let i = 0; i < byteString.length; i++) {
//    ia[i] = byteString.charCodeAt(i);
//  }
//  let bb = new Blob([ab], { "type": mimeString });
//  return bb;
//}


//!function (t, e) { "object" == typeof exports && "object" == typeof module ? module.exports = e() : "function" == typeof define && define.amd ? define([], e) : "object" == typeof exports ? exports.imageConversion = e() : t.imageConversion = e() }(this, (function () {
//  return function (t) {
//    var e = {};
//    function n(r) {
//      if (e[r]) return e[r].exports; var o = e[r] = { i: r, l: !1, exports: {} }; return t[r].call(o.exports, o, o.exports, n), o.l = !0, o.exports
//    } return n.m = t, n.c = e, n.d = function (t, e, r) {
//      n.o(t, e) || Object.defineProperty(t, e, { enumerable: !0, get: r })
//    }, n.r = function (t) {
//      "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(t, Symbol.toStringTag, { value: "Module" }), Object.defineProperty(t, "__esModule", { value: !0 })
//      }, n.t = function (t, e) {
//      if (1 & e && (t = n(t)), 8 & e) return t; if (4 & e && "object" == typeof t && t && t.__esModule) return t;
//      var r = Object.create(null);
//      if (n.r(r), Object.defineProperty(r, "default", { enumerable: !0, value: t }), 2 & e && "string" != typeof t) for (var o in t) n.d(r, o, function (e) { return t[e] }.bind(null, o)); return r
//      }, n.n = function (t) {
//        var e = t && t.__esModule ? function () { return t.default } : function () { return t }; return n.d(e, "a", e), e
//      }, n.o = function (t, e) { return Object.prototype.hasOwnProperty.call(t, e) }, n.p = "", n(n.s = 0)
//  }([function (t, e, n) {
//    "use strict";
//    var r;
//    function o(t) {
//      return ["image/png", "image/jpeg", "image/gif"].some(e => e === t)
//    } n.r(e), n.d(e, "canvastoDataURL", (function () { return a })), n.d(e, "canvastoFile", (function () { return c })), n.d(e, "dataURLtoFile", (function () { return s })), n.d(e, "dataURLtoImage", (function () { return l })), n.d(e, "downloadFile", (function () { return d })), n.d(e, "filetoDataURL", (function () { return f })), n.d(e, "imagetoCanvas", (function () { return g })), n.d(e, "urltoBlob", (function () { return w })), n.d(e, "urltoImage", (function () { return m })), n.d(e, "compress", (function () { return p })), n.d(e, "compressAccurately", (function () { return b })), n.d(e, "EImageType", (function () { return r })), function (t) { t.PNG = "image/png", t.JPEG = "image/jpeg", t.GIF = "image/gif" }(r || (r = {})); var i = function (t, e, n, r) { return new (n || (n = Promise))((function (o, i) { function a(t) { try { u(r.next(t)) } catch (t) { i(t) } } function c(t) { try { u(r.throw(t)) } catch (t) { i(t) } } function u(t) { var e; t.done ? o(t.value) : (e = t.value, e instanceof n ? e : new n((function (t) { t(e) }))).then(a, c) } u((r = r.apply(t, e || [])).next()) })) }; function a(t, e = .92, n = r.JPEG) { return i(this, void 0, void 0, (function* () { return o(n) || (n = r.JPEG), t.toDataURL(n, e) })) } function c(t, e = .92, n = r.JPEG) { return new Promise(r => t.toBlob(t => r(t), n, e)) } var u = function (t, e, n, r) { return new (n || (n = Promise))((function (o, i) { function a(t) { try { u(r.next(t)) } catch (t) { i(t) } } function c(t) { try { u(r.throw(t)) } catch (t) { i(t) } } function u(t) { var e; t.done ? o(t.value) : (e = t.value, e instanceof n ? e : new n((function (t) { t(e) }))).then(a, c) } u((r = r.apply(t, e || [])).next()) })) }; function s(t, e) { return u(this, void 0, void 0, (function* () { const n = t.split(","); let r = n[0].match(/:(.*?);/)[1]; const i = atob(n[1]); let a = i.length; const c = new Uint8Array(a); for (; a--;)c[a] = i.charCodeAt(a); return o(e) && (r = e), new Blob([c], { type: r }) })) } function l(t) { return new Promise((e, n) => { const r = new Image; r.onload = () => e(r), r.onerror = () => n(new Error("dataURLtoImage(): dataURL is illegal")), r.src = t }) } function d(t, e) { const n = document.createElement("a"); n.href = window.URL.createObjectURL(t), n.download = e || Date.now().toString(36), document.body.appendChild(n); const r = document.createEvent("MouseEvents"); r.initEvent("click", !1, !1), n.dispatchEvent(r), document.body.removeChild(n) } function f(t) { return new Promise(e => { const n = new FileReader; n.onloadend = t => e(t.target.result), n.readAsDataURL(t) }) } var h = function (t, e, n, r) { return new (n || (n = Promise))((function (o, i) { function a(t) { try { u(r.next(t)) } catch (t) { i(t) } } function c(t) { try { u(r.throw(t)) } catch (t) { i(t) } } function u(t) { var e; t.done ? o(t.value) : (e = t.value, e instanceof n ? e : new n((function (t) { t(e) }))).then(a, c) } u((r = r.apply(t, e || [])).next()) })) }; function g(t, e = {}) {
//      return h(this, void 0, void 0, (function* () {
//        const n = Object.assign({}, e), r = document.createElement("canvas"), o = r.getContext("2d"); let i, a; for (const t in n) Object.prototype.hasOwnProperty.call(n, t) && (n[t] = Number(n[t]));
//        if (n.scale) {
//          const e = n.scale > 0 && n.scale < 10 ? n.scale : 1;
//          a = t.width * e, i = t.height * e
//        } else
//          a = n.width || n.height * t.width / t.height || t.width, i = n.height || n.width * t.height / t.width || t.height;
//        switch ([5, 6, 7, 8].some(t => t === n.orientation) ? (r.height = a, r.width = i) :
//          (r.height = i, r.width = a), n.orientation) {
//            case 3: o.rotate(180 * Math.PI / 180), o.drawImage(t, -r.width, -r.height, r.width, r.height); break; case 6: o.rotate(90 * Math.PI / 180), o.drawImage(t, 0, -r.width, r.height, r.width); break; case 8: o.rotate(270 * Math.PI / 180), o.drawImage(t, -r.height, 0, r.height, r.width); break;
//          case 2: o.translate(r.width, 0), o.scale(-1, 1), o.drawImage(t, 0, 0, r.width, r.height); break;
//          case 4: o.translate(r.width, 0), o.scale(-1, 1), o.rotate(180 * Math.PI / 180), o.drawImage(t, -r.width, -r.height, r.width, r.height); break;
//          case 5: o.translate(r.width, 0), o.scale(-1, 1), o.rotate(90 * Math.PI / 180), o.drawImage(t, 0, -r.width, r.height, r.width); break;
//          case 7: o.translate(r.width, 0), o.scale(-1, 1), o.rotate(270 * Math.PI / 180), o.drawImage(t, -r.height, 0, r.height, r.width); break;
//          default: o.drawImage(t, 0, 0, r.width, r.height)
//        }return r
//      }))
//    } function w(t) { return fetch(t).then(t => t.blob()) } function m(t) { return new Promise((e, n) => { const r = new Image; r.onload = () => e(r), r.onerror = () => n(new Error("urltoImage(): Image failed to load, please check the image URL")), r.src = t }) } var y = function (t, e, n, r) { return new (n || (n = Promise))((function (o, i) { function a(t) { try { u(r.next(t)) } catch (t) { i(t) } } function c(t) { try { u(r.throw(t)) } catch (t) { i(t) } } function u(t) { var e; t.done ? o(t.value) : (e = t.value, e instanceof n ? e : new n((function (t) { t(e) }))).then(a, c) } u((r = r.apply(t, e || [])).next()) })) }; function p(t, e = {}) { return y(this, void 0, void 0, (function* () { if (!(t instanceof Blob)) throw new Error("compress(): First arg must be a Blob object or a File object."); if ("object" != typeof e && (e = Object.assign({ quality: e })), e.quality = Number(e.quality), Number.isNaN(e.quality)) return t; const n = yield f(t); let i = n.split(",")[0].match(/:(.*?);/)[1], c = r.JPEG; o(e.type) && (c = e.type, i = e.type); const u = yield l(n), d = yield g(u, Object.assign({}, e)), h = yield a(d, e.quality, c), w = yield s(h, i); return w.size > t.size ? t : w })) } function b(t, e = {}) { return y(this, void 0, void 0, (function* () { if (!(t instanceof Blob)) throw new Error("compressAccurately(): First arg must be a Blob object or a File object."); if ("object" != typeof e && (e = Object.assign({ size: e })), e.size = Number(e.size), Number.isNaN(e.size)) return t; if (1024 * e.size > t.size) return t; e.accuracy = Number(e.accuracy), (!e.accuracy || e.accuracy < .8 || e.accuracy > .99) && (e.accuracy = .95); const n = e.size * (2 - e.accuracy) * 1024, i = 1024 * e.size, c = e.size * e.accuracy * 1024, u = yield f(t); let d = u.split(",")[0].match(/:(.*?);/)[1], h = r.JPEG; o(e.type) && (h = e.type, d = e.type); const w = yield l(u), m = yield g(w, Object.assign({}, e)); let y, p = .5; const b = [null, null]; for (let t = 1; t <= 7; t++) { y = yield a(m, p, h); const e = .75 * y.length; if (7 === t) { (n < e || c > e) && (y = [y, ...b].filter(t => t).sort((t, e) => Math.abs(.75 * t.length - i) - Math.abs(.75 * e.length - i))[0]); break } if (n < e) b[1] = y, p -= Math.pow(.5, t + 1); else { if (!(c > e)) break; b[0] = y, p += Math.pow(.5, t + 1) } } const v = yield s(y, d); return v.size > t.size ? t : v })) }
//  }])
//}));
