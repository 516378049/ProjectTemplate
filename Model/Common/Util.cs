/**** 
 * Desc: Common variable 
 * Author: changchun
 *****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public abstract class Util
    {

        # region 系统通用常量定义
        public const string ErrCode_Pre_Core = "-1101";
        public const string ErrCode_Pre_DataAccess = "-1102";
        public const string ErrCode_Pre_Business = "-1103";
        public const string ErrCode_Pre_Web_MainWeb = "-1201";
        public const string ErrCode_Pre_AdminWeb = "-1301";
        #endregion

        #region Control
        public static string ctl_Search_StartDate = "txtStart";
        public static string ctl_Search_EndDate = "txtEnd";
        public static string ctl_Search_Category = "slCategory";
        public static string ctl_Search_SKU = "taSKU";
        public static string ctl_Search_Template = "slTemplate";
        public const string ctl_Search_ContractSeason = "slClc";
        public const string ctl_Search_CategorySystem = "slCategorySys";
        public const string ctl_Search_SKUSystem = "taSKUSys";
        public const string ctl_Search_TabValue = "tabValue";
        public const string ctl_Search_GenderGroup = "slGender";
        public const string ctl_Search_SysGenderGroup = "slSysGender";
        public const string ctl_MLP_Gate = "slGate";
        public static string ctl_Del_Flag = "OpsFlag";
        public static string ctl_Login_UserId = "txtUserId";
        public static string ctl_Login_Password = "txtPassword";
        #endregion

        #region Log
        public static string s_Logs_Folder_Path_Key = "LogsFolder";
        public static string s_DTC_Log_FileName = "DTC_Log_{0}_{1}_{2}.log";
        #endregion

        #region SP
        public static string sp_DGTL_MMX = "SP_DGTL_ExportTemplateMMX";
        public const string sp_DGTL_MMX_NEW = "SP_DGTL_DownloadMMX";
        public static string sp_DGTL_Allocation_Feedback = "sp_DGTL_AF_Proc";
        public static string sp_DGTL_Digital_Merch = "sp_DGTL_DM_Proc";
        public static string sp_DGTL_OPS_Template = "sp_DGTL_OT_Proc";
        public static string sp_DGTL_Category = "SP_DGTL_GetCategory";
        public const string sp_DGTL_MMXSysCategory = "SP_DGTL_GetMMXSYSCategory";
        public const string sp_DGTL_TechProd = "SP_DGTL_GetTechProd";
        public const string sp_DGTL_GenderGroup = "SP_DGTL_GetGenderGroup";
        public const string sp_DGTL_SysGenderGroup = "SP_DGTL_GetSysGenderGroup";
        public const string sp_DGTL_ContractSesn = "SP_DGTL_GetContractSeasonFromMMX";
        public const string sp_DGTL_GetModificationData = "SP_DGTL_GetModificationData";
        public const string sp_DGTL_GetModificationDataForEdit = "SP_DGTL_GetModificationDataForEdit";
        public const string sp_DGTL_GetDamData = "SP_DGTL_GetDamData";
        public const string sp_DGTL_GetProdCopyData = "SP_DGTL_GetProdCopyData";
        public static string sp_DGTL_GetOMDDateRange = "SP_DGTL_GetOMDDateRange";
        public const string sp_DGTL_GetCNLaunchTrackData = "SP_DGTL_GetCNLaunchTrackData";
        public static string sp_DGTL_ETL_MMX_BatchId = "SP_DGTL_ETL_START";
        public static string sp_DGTL_ETL_MMX_TNF = "SP_DGTL_ETL_MMX_TNF"; 
        public static string sp_DGTL_ETL_MMX_DW = "SP_DGTL_ETL_MMX_DW";
        public static string sp_DGTL_ETL_AMO_INF = "SP_DGTL_ETL_MList_Alloc_Merch_OPS_TNF";
        public static string sp_DGTL_ETL_Alloc_DW = "SP_DGTL_ETL_MList_Alloc_DW";
        public static string sp_DGTL_ETL_Merch_DW = "SP_DGTL_ETL_MList_Merch_DW";
        public static string sp_DGTL_ETL_OPS_DW = "SP_DGTL_ETL_MList_Ops_DW";
        public const string sp_DGTL_TECH_PRODUCTION_TNF = "SP_DGTL_TECH_PRODUCTION_TNF";
        public const string sp_DGTL_TECH_PRODUCTION_DW = "SP_DGTL_TECH_PRODUCTION_DW";
        public const string sp_DGTL_DAM_TNF = "SP_DGTL_DAM_TNF";
        public const string sp_DGTL_DAM_DW = "SP_DGTL_DAM_DW";
        public const string sp_DGTL_ProdCopy_TNF = "SP_DGTL_PRODCOPY_TNF";
        public const string sp_DGTL_ProdCopy_DW = "SP_DGTL_PRODCOPY_DW";
        public const string sp_DGTL_MLP_DW = "SP_DGTL_MLP_DW";
        public const string SP_DGTL_CNLaunchTrack = "SP_DGTL_CNLaunchTrack_DW";
        public static string sp_DGTL_MMX_LOG = "SP_DGTL_GetMMXSTG";
        public static string sp_DGTL_ALLOC_LOG = "SP_DGTL_Get ALLOCOPTSTG";
        public const string sp_DGTL_COPY_LOG = "SP_DGTL_GetProductCopySTG";
        public const string sp_DGTL_GETMLP = "SP_DGTL_GetMLData";
        public const string sp_DGTL_MLPGender = "SP_DGTL_GetMLPGender";
        public const string sp_DGTL_MLPCategory = "SP_DGTL_GetMLPCategory";
        public const string sp_DGTL_MLPGate = "SP_DGTL_GetMLPGate";
        public const string sp_DGTL_GetMLPDelete = "SP_DGTL_GetMLPDeleteData";
        public const string sp_DGTL_DeleteMLPByCodeGate = "SP_DGTL_DeleteMLPByCodeGate";
        public const string sp_DGTL_DeleteMLPByGate = "SP_DGTL_DeleteMLPByGate";
        public static string sp_DGTL_DEL_GetProd = "SP_DGTL_GetProdByCodes";
        public static string sp_DTTL_DEL_Proc = "SP_DGTL_DelMMXByCodes";
        public const string sp_DGTL_DownloadMaster = "SP_DGTL_DownloadMasterList";
        public const string SP_GetMMXValidation = "SP_DGTL_GetMMXValidationDim";
        public const string sp_DGTL_MMX_MLP_DW = "SP_DGTL_MMX_MLP_DW";
        public const string sp_GetMMXMLPData = "SP_DGTL_GetMMXMLPData";
        public const string sp_DGTL_TMALL_LAUNCHTRACK_DW = "SP_DGTL_TMALL_LAUNCHTRACK_DW";
        public const string SP_DGTL_GetTmallLaunchTrack = "SP_DGTL_GetTmallLaunchTrack";
        public const string SP_DGTL_GetSearchPageData = "SP_DGTL_GetSearchPageData";
        public const string SP_DGTL_SNKRS_THREAD_DW = "SP_DGTL_SNKRS_THREAD_DW";
        public const string SP_DGTL_HK_LAUNCHTRACK_DW = "SP_DGTL_HK_LAUNCHTRACK_DW";
        //Add by changchun 20180906
        public const string SP_DGTL_WECHAT_MEMBERSHIP_DW = "SP_DGTL_WECHAT_MEMBERSHIP_DW";
        public const string SP_DGTL_DATA_GOVERNANCE_DW = "SP_DGTL_DATA_GOVERNANCE_DW";
        #endregion

        #region Table
        public static string tbl_DGTL_MMX_TempTable = "[CRB_T_STG].[dbo].[DGTL_MMX_STG]";
        public static string sp_DGTL_AMO_TempTable = "[CRB_T_STG].[dbo].[DGTL_MLIST_ALLOC_MERCH_OPS_STG]";
        public const string sp_DGTL_DAM_TempTable = "[CRB_T_STG].[dbo].[DGTL_DAM_STG]";
        public const string sp_DGTL_ProdCopy_Template = "[CRB_T_STG].[dbo].[DGTL_PRODUCTCOPY_STG]";
        public const string sp_DGTL_MLP_Template = "[CRB_T_STG].[dbo].[DGTL_MLP_STG]";
        public const string sp_DGTL_CNLaunchTrack_Template = "[CRB_T_STG].[dbo].[DGTL_CNLAUNCHTRACK_STG]";
        public const string TABLE_DGTL_MLP_MMX_STG = "CRB_T_STG.DBO.DGTL_MMX_MLP_STG";
        public const string TABLE_DOWNLOADMASTER = "DGTL_MASTERLIST_DOWNLOAD";
        public const string TABLE_DOWNLOADCOMPARE = "DGTL_MASTER_COMPARE";
        public const string TABLE_DGTL_TMALL_LAUNCHTRACK_STG = "CRB_T_STG.DBO.DGTL_TMALL_LAUNCHTRACK_STG";
        public const string TABLE_DGTL_SNKRS_THREAD_TRACK_STG = "CRB_T_STG.DBO.DGTL_SNKRS_THREAD_TRACK_STG";
        public const string TABLE_DGTL_HK_LAUNCHTRACK_STG = "CRB_T_STG.DBO.DGTL_HK_LAUNCHTRACK_STG";
        public const string TABLE_DGTL_PERFORMANCE_TRACK_FACT = "DGTL_PERFORMANCE_TRACK_FACT";
        public const string TABLE_DGTL_WECHAT_MEMBERSHIP_STG = "CRB_T_STG.DBO.DGTL_WECHAT_MEMBERSHIP_STG";
        public const string TABLE_DGTL_DGTL_DATA_GOVERNANCE_STG = "CRB_T_STG.DBO.DGTL_DATA_GOVERNANCE_STG";
        #endregion

        #region View
        public const string VW_DGTL_SNKRS_THREAD = "VW_DGTL_SNKRS_THREAD";
        public const string VW_DGTL_CN_LAUNCH_TRACK = "VW_DGTL_CN_LAUNCH_TRACK";
        public const string VW_DGTL_HK_LAUNCH_TRACK = "VW_DGTL_HK_LAUNCH_TRACK";
        public const string VW_DGTL_DAM = "VW_DGTL_DAM";
        public const string VW_DGTL_PERFORMANCE = "VW_DGTL_PERFORMANCE_TRACK";
        public const string VW_DGTL_TMALL_LAUNCHTRACK = "VW_DGTL_TMALL_LAUNCHTRACK";
        public const string VW_DGTL_TECHPROD = "VW_DGTL_TECHPROD";
        public const string VW_DGTL_PRODUCTCOPY = "VW_DGTL_PRODUCTCOPY";
        // update by changchun
        public const string VW_DGTL_Wechat = "VW_DGTL_Wechat";
        public const string VW_DGTL_DATA_GOVERNANCE = "VW_DGTL_DATA_GOVERNANCE";
        #endregion

        #region Template File Name
        public static string s_upload_allow_file_mmx = "Product_Information";
        public static string s_upload_allow_file_allocation_feedback = "Digital-Master-List_allocation-feedback";
        public static string s_upload_allow_file_digital_merch = "Landing_Page_Key_Products_Tracking_List";
        public static string s_upload_allow_file_OPS_template = "Digital-Master-List-OPS";
        public const string s_upload_allow_file_Tech_Production = "Tech_Production_Tracking_List";
        public const string s_upload_allow_dam = "Digital_Assets_Management_Tracking_List";
        public const string s_upload_allow_file_prodcopy = "Localization_Product_Copy_Tracking_List";
        public const string s_upload_mlp = "MLP";
        public const string s_upload_cnlaunchtrack = "CNLaunchTrack";
        public const string s_upload_tmall_launch_track = "Tmall-Launch-Track";
        public const string dtc_master_list_template_sheetname = "Migital-Master-List-Template";
        public const string s_upload_snkrs_thread = "SNKRS_Thread_Readiness_Tracking_List";
        public const string s_upload_hk_launch_track = "HK-Launch-Track";
        public const string s_upload_performance_track = "Performance-Track-List";

        public const string FILE_TEMPLATE_MLP = "MLP";
        public const string FILE_TEMPLATE_MERCH = "Landing_Page_Key_Products_Tracking_List";
        public const string FILE_TEMPLATE_OPS = "Digital-Master-List-OPS_template";
        public const string FILE_TEMPLATE_TECH = "Tech_Production_Tracking_List";
        public const string FILE_TEMPLATE_DAM = "Digital_Assets_Management_Tracking_List";
        public const string FILE_TEMPLATE_PRODCOPY = "Localization_Product_Copy_Tracking_List";
        public const string FILE_TEMPLATE_CNLAUNCHTRACK = "CNLaunchTrack";
        public const string FILE_TEMPLATE_TMALLLAUNCHTRACK = "Tmall-Launch-Track";
        public const string FILE_TEMPLATE_SNKRSLAUNCHTRACK = "SNKRS_Thread_Readiness_Tracking_List";
        public const string FILE_TEMPLATE_HKLAUNCHTRACK = "HK-Launch-Track";
        public const string FILE_TEMPLATE_PERFORMANCETRACK = "Performance-Track-List";
        public const string FILE_TEMPLATE_WECHATLAUNCH = "MEMBERSHIP";
        public const string FILE_TEMPLATE_DATAGOVERNANCE = "DATA_GOVERNANCE_TRACKING_LIST"; 
        #endregion

        #region HTML
        public static string s_result_table = "<table id='validateResErrData' class='validate_result'>{0}</table>";
        public static string s_table_row_header = "<tr class='trHeader'>";
        public static string s_table_row_start = "<tr>";
        public static string s_table_row_err_start = "<tr class='ms_row_err'>";
        public static string s_table_row_omd_err_start = "<tr class='ms_row_omd_err'>";
        public static string s_table_row_end = "</tr>";
        public static string s_table_row_td = "<td>{0}</td>";
        public static string s_table_row_err_td = "<td class='ms_validate_err'>{0}</td>";
        public static string s_table_row_title = "<th align='center' class='ms_title' style='vertical-align: middle;'>{0}</th>";
        public static string s_Legal_UserNames_Key = "LegalUserNames";
        public static string s_header_format = "<thead>{0}</thead>";
        public static string s_tbody_start = "<tbody>";
        public static string s_tbody_end = "</tbody>";
        public const string MODIFICATION_EDTI_TABLE = "<table id='etable' class='edittable'>{0}</table>";
        public const string MODIFICATION_EDIT_ROW = "<tr>{0}</tr>";
        public const string MODIFICATION_EDIT_COLUMN = "<td>{0}</td>";
        public const string MODIFICATIONEDITTITLE = "<th>{0}</th>";
        public const string MODIFICATION_INPUT = "<input class='easyui-textbox' name='{0}' value='{1}' style='width:100%;height:32px'>";
        public const string MODIFICATION_DATE_INPUT = "<input class='easyui-datebox' type='text' name='{0}' value='{1}'/>";
        public const string MODIFICATION_INPUT_NOTENABLE = "<input class='forminput' disabled='disabled'  type='text' name='{0}' value='{1}'/>";
        public const string DIV_TAG = "<div>{0}</div>";
        public const string DIFFERENT_TEMPLATE_ERROR = "Please select the column in the same template";
        public const string MODIFICATION_INPUT_HIDDEN = "<input hidden='hidden' name='Templatetype' class='forminput' value='{0}'/>";
        public const string MODIFICATION_EASYUI_INPUT = "<input class='easyui-textbox' name='{0}' value='{1}' style='width:100%;height:32px'>";
        public const string MODIFICATION_EASYUI_INPUT_MULTI = "<input class='easyui-textbox' data-options='multiline:true' name='{0}' value='{1}' style='width:100%;height:100px'>";
        public const string MODIFICATION_EASYUI_INPUT_NUMBER = "<input class='easyui-numberbox' name='{0}' value='{1}'></input>";
        public const string MODIFICATION_EASYUI_INPUT_NUMBER_RANGE = "<input class='easyui-numberbox' name='{0}' data-options='min:{3},max:{4},precision:{2}' value='{1}'></input>";
        public const string MODIFICATION_SELECT_YN = "<select class='easyui-combobox' style='width:230px' panelHeight='43' editable='false' name='{0}'><option {1} value=''>----Please Choose----</option><option {2} value='Y'>Y</option></select>";
        public const string MODIFICATION_SELECT = "<select class='easyui-combobox' panelHeight='100' style='width:230px' editable='false' name='{0}'>{1}</select>";
        public const string MODIFICATION_SELECT_OPTION = "<option {0} value='{1}'>{1}</option>";
        #endregion

        #region Config
        public static string AD_Name = "DomainName";
        public static string LDAPAddress = "LDAPAddress";
        public static string e_MailHost_key = "MailHost";
        public static string e_AuthUser_key = "AuthUser";
        public static string e_To_key = "EmailTo";
        public static string e_CC_key = "EmailCC";
        public static string e_AuthPassword_key = "AuthPassword";
        public static string e_MailSender_key = "MailSender";
        public static string e_subject_formatstring = "ChinaBI_Load_{0}_Fail";
        public static string e_body_formating = "Below template data is failed to load into DB:<br />" +
                                                                  "Template name: {0} <br />" +
                                                                   "Batch Id: {1} <br />" +
                                                                  "Data Load Time: {2} <br /><br /><br />" +
                                                                  "Please take action to check.";
        #endregion

        #region Template Alia
        public const string MMX_ALIAS = "Prod";
        public const string TECH_ALIAS = "Tech";
        public const string OPS_ALIAS = "Ops";
        public const string MERCH_ALIAS = "Merch";
        #endregion

        #region Download Page Condition
        public const string CN_SLP_YES = "CNSLPY";
        public const string CN_SLP_NO = "CNSLPN";
        public const string HK_SLP_YES = "HKSLPY";
        public const string HK_SLP_NO = "HKSLPN";
        public const string TMALL_SLP_YES = "TmallSLPY";
        public const string TMALL_SLP_NO = "TmallSLPN";
        public const string QS_YES = "QSY";
        public const string QS_NO = "QSN";
        public const string HARD_LAUNCH_YES = "HardLaunchY";
        public const string HARD_LAUNCH_NO = "HardLaunchN";
        public const string TOP_YES = "CNTopY";
        public const string TOP_NO = "CNTopN";
        public const string LAB_YES = "LabY";
        public const string LAB_NO = "LabN";
        public const string CN_INVY_YES = "CNINVYY";
        public const string CN_INVY_NO = "CNINVYN";
        public const string CN_IMG_YES = "CNIMGY";
        public const string CN_IMG_NO = "CNIMGN";
        public const string CN_EN_COPY_YES = "CNENY";
        public const string CN_EN_COPY_NO = "CNENN";
        public const string CN_CN_COPY_YES = "CNCNY";
        public const string CN_CN_COPY_NO = "CNCNN";
        public const string CN_ACTIVE_ACT = "CNActiveActive";
        public const string CN_ACTIVE_HOLD = "CNActiveHold";
        public const string CN_ACTIVE_NOTAPPROVE = "CNActiveNA";
        public const string CN_ACTIVE_CLOSEOUT = "CNActiveCloseOut";
        public const string CN_ACTIVE_CANCEL = "CNActiveCancel";
        public const string CN_HIS_ONSITE_YES = "CNHisOnsiteY";
        public const string CN_HIS_ONSITE_NO = "CNHisOnsiteN";

        public const string HK_INVY_YES = "HKINVYY";
        public const string HK_INVY_NO = "HKINVYN";
        public const string HK_IMG_YES = "HKIMGY";
        public const string HK_IMG_NO = "HKIMGN";
        public const string HK_EN_COPY_YES = "HKENY";
        public const string HK_EN_COPY_NO = "HKENN";
        public const string HK_CN_COPY_YES = "HKCNY";
        public const string HK_CN_COPY_NO = "HKCNN";
        public const string HK_ACTIVE_YES = "HKActiveY";
        public const string HK_ACTIVE_NO = "HKActiveN";
        public const string HK_HIS_ONSITE_YES = "HKHisOnsiteY";
        public const string HK_HIS_ONSITE_NO = "HKHisOnsiteN";

        public const string TMALL_INVY_YES = "TmallINVYY";
        public const string TMALL_INVY_NO = "TmallINVYN";
        public const string TMALL_IMG_YES = "TmallIMGY";
        public const string TMALL_IMG_NO = "TmallIMGN";
        public const string TMALL_EN_COPY_YES = "TmallENY";
        public const string TMALL_EN_COPY_NO = "TmallENN";
        public const string TMALL_CN_COPY_YES = "TmallCNY";
        public const string TMALL_CN_COPY_NO = "TmallCNN";
        public const string TMALL_ACTIVE_YES = "TmallActiveY";
        public const string TMALL_ACTIVE_NO = "TmallActiveN";
        public const string TMALL_HIS_ONSITE_YES = "TmallHisOnsiteY";
        public const string TMALL_HIS_ONSITE_NO = "TmallHisOnsiteN";

        public const string CN_BP_YES = "CNBPY";
        public const string CN_BP_NO = "CNBPN";
        public const string HK_BP_YES = "HKBPY";
        public const string HK_BP_NO = "HKBPN";
        public const string TMALL_BP_YES = "TmallBPY";
        public const string TMALL_BP_NO = "TmallBPN";

        public const string CN_KEY_PRODUCT_YES = "CNKeyProY";
        public const string CN_KEY_PRODUCT_NO = "CNKeyProN";
        public const string HK_KEY_PRODUCT_YES = "HKKeyProY";
        public const string HK_KEY_PRODUCT_NO = "HKKeyProN";
        public const string TMALL_KEY_PRODUCT_YES = "TmallKeyProY";
        public const string TMALL_KEY_PRODUCT_NO = "TmallKeyProN";

        public const string CN_FLOW_PRODUCT_YES = "CNFlowProY";
        public const string CN_FLOW_PRODUCT_NO = "CNFlowProN";
        public const string HK_FLOW_PRODUCT_YES = "HKFlowProY";
        public const string HK_FLOW_PRODUCT_NO = "HKFlowProN";
        public const string TMALL_FLOW_PRODUCT_YES = "TmallFlowProY";
        public const string TMALL_FLOW_PRODUCT_NO = "TmallFlowProN";

        public const string GOOD_LEVEL_YES = "GoodLevelY";
        public const string GOOD_LEVEL_NO = "GoodLevelN";
        public const string BUY_MASTER_YES = "BuyMasterY";
        public const string BUY_MASTER_NO = "BuyMasterN";
        public const string CN_EXCLUSIVE_YES = "CNExclusiveY";
        public const string CN_EXCLUSIVE_NO = "CNExclusiveN";
        public const string HK_TOP_STYLE_YES = "HKTopY";
        public const string HK_TOP_STYLE_NO = "HKTopN";
        public const string HK_EXCLUSIVE_YES = "HKExclusiveY";
        public const string HK_EXCLUSIVE_NO = "HKExclusiveN";
        public const string TMALL_TOP_STYLE_YES = "TmallTopY";
        public const string TMALL_TOP_STYLE_NO = "TmallTopN";
        public const string TMALL_EXCLUSIVE_YES = "TmallExclusiveY";
        public const string TMALL_EXCLUSIVE_NO = "TmallExclusiveN";
        #endregion

        #region Permission
        public const string UPLOAD_PERMISSION = "Upload";
        public const string DELETE_PERMISSION = "Delete";
        public const string MODIFICATION_PERMISSION = "Modification";
        public const string MLP_PERMISSION = "MLP";
        public const string ADMIN = "Admin";
        public const string UPLOAD_USER = "UploadUser";
        public const string DELETE_USER = "DeleteUser";
        public const string MODIFICATION_USER = "ModificationUser";
        public const string MLP_USER = "MLPUser";
        #endregion

        #region Column Name
        public const string Product_Code = "ProductCode";
        public const string Col_Name_MMX_CN_COM = "CnComActualBuy";
        public const string Col_Name_MMX_HK_COM = "HkComActualBuy";
        public const string Col_Name_MMX_Tmall = "TmallActualBuy";
        public const string Col_Name_CN_NSO = "CNNSOActualBuy";
        public const string Col_Name_HK_NSO = "HKNSOActualBuy";
        public const string Col_Name_TW_NSO = "TWNSOActualBuy";
        public const string Col_Name_MMX_Tmall_Jordan = "TmallJordanActualBuy";
        public const string Col_Name_MMX_Tmall_Ya = "TmallYaActualBuy";//update by changchun 20180405
        public const string Col_Name_MMX_ProductInfoOwner = "ProductInfoOwner";
        public const string Col_Name_Gate = "Gate";
        public const string Col_Name_MLP_CNNSOBUYUNITS = "CNNSOBuyUnits";
        public const string Col_Name_MLP_CNDIGITALBUYUNITS = "CnDigitalBuyUnits";
        public const string Col_Name_MLP_HKNSOBUYUNITS = "HKNSOBuyUnits";
        public const string Col_Name_MLP_HKCOMBUYUNITS = "HkComBuyUnits";
        public const string Col_Name_MLP_TWNSOBUYUNITS = "TWNSOBuyUnits";
        public const string Col_Name_Pro_Metal = "ProductLevel";
        public const string Col_Name_GC_FST_DAY = "GcFirstDayStPert";
        public const string Col_Name_GC_FST_WEEK = "GcFirstWeekStPert";
        public const string Col_Name_GC_FST_MONTH = "GcFirstMonthStPert";
        public const string Col_Name_TMALL_JORDAN_SPECS_REVIEW = "TmallJordanSpecsReview";
        public const string Col_Name_TMALL_JORDAN_COPY_WRITING = "TmallJordanCopyWriting";
        public const string Col_Name_TMALL_YA_SPECS_REVIEW = "TmallYaSpecsReview";//update by changchun 20180405
        public const string Col_Name_TMALL_YA_COPY_WRITING = "TmallYaCopyWriting";//update by changchun 20180405
        #endregion

        #region flag
        public const string MMX_CN_COM_OMD = "CNCOMOMD";
        public const string MMX_HK_COM_OMD = "HKCOMOMD";
        public const string MMX_TMall_COM_OMD = "TMALLCOMOMD";
        public const string MMX_CN_NSO_OMD = "CNNSOOMD";
        public const string MMX_HK_NSO_OMD = "HKNSOOMD";
        public const string MMX_TW_NSO_OMD = "TWNSOOMD";
        public const string MMX_TMALL_JORDAN_OMD = "TMALLJORDANOMD";
        public const string MMX_TMALL_YA_OMD = "TMALLYAOMD";//update by changchun 20180405
        public const string CATEGORY = "CATEGORY";
        public const string DIVISION = "DIVISION";
        public const string GENDER_GROUP = "GENDER";
        public const string GENDER_AGE = "AGE";
        public const string FLOW = "FLOW";
        public const string PROD_TYPE = "ProdType";
        public const string CN_MSRP = "CNMSRP";
        public const string HK_MSRP = "HKMSRP";
        public const string TW_MSRP = "TWMSRP";
        public const string COLOR_DESC = "COLORDESCRIPTION";
        public const string GC_FIRST_DAY_ST_PERT = "GCFIRSTDAYSTPERT";
        public const string GC_FIRST_WEEK_ST_PERT = "GCFIRSTWEEKSTPERT";
        public const string GC_FIRST_MONTH_ST_PERT = "GCFIRSTMONTHSTPERT";
        public const string CNCOM_TAKE_OFF_TIME = "CNCOMTAKEOFFTIME";
        public const string TMALL_TAKE_OFF_TIME = "TMALLTAKEOFFTIME";
        public const string TMALL_JORDAN_SPECS_REVIEW = "TMALLJORDANSPECSREVIEW";
        public const string TMALL_JORDAN_COPY_WRITING = "TMALLJORDANCOPYWRITING";
        public const string TMALL_YA_SPECS_REVIEW = "TMALLYASPECSREVIEW";//update by changchun 20180405
        public const string TMALL_YA_COPY_WRITING = "TMALLYACOPYWRITING";//update by changchun 20180405
        #endregion

        #region Value
        public const string VAL_PRO_LEVEL_GOLD = "GOLD";
        public const string VAL_PRO_LEVEL_PLATINUM = "PLATINUM";
        public const string VAL_PRO_LEVEL_SILVER = "SILVER";
        public const string VAL_PRO_LEVEL_BRAND = "BRAND";
        #endregion

        #region Validate Value
        public const string STR_MMX_CATEGORY = "ACTION OUTDOOR;BASKETBALL;FOOTBALL, BASEBALL, AT;FOOTBALL/SOCCER;GOLF;JORDAN;NIKE SPORTSWEAR;OTHER;RUNNING;TENNIS;WOMEN TRAINING;YOUNG ATHLETES";
        public const string STR_MMX_PRODUCT_TYPE = "APPAREL;FOOTWEAR;EQUIPMENT";
        public const string STR_MMX_GENDER_GROUP = "KIDS;MENS;WOMENS";
        public const string STR_MMX_GENDER_AGE = "ADULT UNISEX;BOYS;BOYS GRADE SCHL;BOYS INFANT;BOYS PRE SCHOOL;BOYS TODDLER;CHILD UNISEX;GIRL GRADE SCHL;GIRL PRE SCHOOL;GIRLS;GIRLS INFANT;GIRLS TODDLER;GRD SCHOOL UNSX;INFANT UNISEX;INFANTS;LITTLE BOYS;LITTLE GIRLS;MENS;PRE SCHOOL UNSX;TODDLER UNISEX;WOMENS;YOUNG MEN;YOUNG WOMEN;YOUTH UNISEX";
        public const string STR_MMX_FLOW = "1;2;3;1&2;2&3;1&2&3;Carry Over";
        public const string STR_MMX_PROD_LEVEL = Util.VAL_PRO_LEVEL_GOLD + ";" + Util.VAL_PRO_LEVEL_BRAND + ";" + Util.VAL_PRO_LEVEL_PLATINUM + ";" + Util.VAL_PRO_LEVEL_SILVER;
        public const string STR_MMX_DEMAND_TYPE = "VH;H;M;L";

        public const string STR_MLP_CATEGORY = "ACTION OUTDOOR;BASKETBALL;FOOTBALL, BASEBALL, AT;FOOTBALL/SOCCER;GOLF;MNSW;WNSW;OTHER;MRUN;WRUN;TENNIS;WOMEN TRAINING;YOUNG ATHLETES;JORDAN";
        public const string STR_MLP_DIVISION = "APPAREL DIVISION;FOOTWEAR DIVISION;EQUIPMENT DIVISION";
        //public const string STR_MLP_GENDER = "MALE;FEMALE;UNISEX;BOYS;GIRLS";
        public const string STR_MLP_GENDER = "MALE;FEMALE;UNISEX";
        public const string STR_MLP_AGE = "ADULT;GS;PS;TD";
        public const string STR_MLP_FLOW = "1;2;3;1&2;2&3;1&2&3;1&3;HD;QS;LAB;TBD";
        #endregion

        #region Message
        public const string MS_TIMEOUT = "TIMEOUT";
        #endregion
    }

}
