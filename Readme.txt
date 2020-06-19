git transfer to gitee sucessfully,congradulation!
All of the Contrller should be inherit BaseController， so  that you can use  BaseController's function
All of the DataAccess should be inherit DBContextBase， so  that you can use  DBContextBase's function 
All of the Bussiness should be inherit BussinessBase， so  that you can use  BussinessBase's function 
1、写日志功能
	1、在方法上加上[UserOptionLog]自定义过滤器
	2、在方法返回数据中使用父类的Json方法格式化数据 eg: return Json("", JsonRequestBehavior.AllowGet);

2、
//usage scenario for access database
for below function , we suggest fllow's scenario
1、ExcuteProEntpriseLib ： excute the sp  with returnValue
2、ExcuteSqlEntpriseLib ： excute the sql  with returnValue
3、getDataTableEntpriseLib ： excute the sp  with DataTable and  ReturnValue and outputs
4、getListEntpriseLibsql ： excute the sql  with List<> 
	
3、write log 
use below sentense
LogManager.DefaultLogger.Info("DefaultLogger");
LogManager.StorageLogger.Info("StorageLogger");

note: there are four level log print likes info、debug 、warn、error
where we print the error log it will send the mail, to do this, we  should set the config for thinkDev.logging.config as below:
    <add key="MailServer" value="fastsmtphz.qiye.163.com"/>
    <add key="FromMail" value="webalarm@rundamedical.com"/>
    <add key="MailAccount" value="webalarm@rundamedical.com"/>
    <add key="MailPassword" value="runda123"/>
    <var name="ToMail" value="wangsiyuan@rundamedical.com;chenchangchun@rundamedical.com"/>