namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class users
    {
        [Key]
        public int User_UserId { get; set; }

        public int? User_PrimaryChannelId { get; set; }

        [StringLength(50)]
        public string User_Logon { get; set; }

        [StringLength(35)]
        public string User_LastName { get; set; }

        [StringLength(20)]
        public string User_FirstName { get; set; }

        [StringLength(5)]
        public string User_Language { get; set; }

        [StringLength(40)]
        public string User_Department { get; set; }

        [StringLength(20)]
        public string User_Phone { get; set; }

        [StringLength(20)]
        public string User_Extension { get; set; }

        [StringLength(20)]
        public string User_Pager { get; set; }

        [StringLength(20)]
        public string User_Homephone { get; set; }

        [StringLength(20)]
        public string User_MobilePhone { get; set; }

        [StringLength(20)]
        public string User_Fax { get; set; }

        [StringLength(255)]
        public string User_EmailAddress { get; set; }

        public DateTime? User_LastLogin { get; set; }

        [StringLength(25)]
        public string User_User1 { get; set; }

        [StringLength(150)]
        public string User_Password { get; set; }

        public int? User_CreatedBy { get; set; }

        public DateTime? User_CreatedDate { get; set; }

        public int? User_UpdatedBy { get; set; }

        public DateTime? User_UpdatedDate { get; set; }

        public DateTime? User_TimeStamp { get; set; }

        public int? User_Deleted { get; set; }

        public int? User_Per_ContactInsert { get; set; }

        public int? User_Per_ContactUpdate { get; set; }

        public int? User_Per_Communication { get; set; }

        public int? User_Per_Opportunity { get; set; }

        public int? User_Per_Case { get; set; }

        public int? User_Per_ToDo { get; set; }

        public int? User_Per_Channels { get; set; }

        public int? User_Per_Reports { get; set; }

        public int? User_Per_Admin { get; set; }

        public int? User_Per_Team { get; set; }

        public int? User_Per_TeamUpdate { get; set; }

        public int? User_Per_TeamSensitive { get; set; }

        public int? User_ProfileEnabled { get; set; }

        [StringLength(128)]
        public string User_Profile { get; set; }

        public DateTime? User_ProfileLastSyncDate { get; set; }

        [StringLength(128)]
        public string User_LastEntitySyncOutcome { get; set; }

        public string User_RecentList { get; set; }

        [StringLength(5)]
        public string User_MustChangePassword { get; set; }

        [StringLength(5)]
        public string User_CannotChangePassword { get; set; }

        public DateTime? User_PasswordUpdateDate { get; set; }

        [StringLength(5)]
        public string User_PasswordNeverExpires { get; set; }

        [StringLength(5)]
        public string User_SMSNotification { get; set; }

        [StringLength(128)]
        public string User_CTIDeviceName { get; set; }

        [StringLength(5)]
        public string User_Disabled { get; set; }

        [StringLength(5)]
        public string User_Resource { get; set; }

        public int? User_Per_CompAssign { get; set; }

        public int? User_Per_EntityMerge { get; set; }

        public int? User_SecurityProfile { get; set; }

        [StringLength(5)]
        public string User_ExternalLogonAllowed { get; set; }

        public int? User_PrimaryTerritory { get; set; }

        [StringLength(5)]
        public string User_IsTerritoryManager { get; set; }

        public int? User_TerritoryProfile { get; set; }

        [StringLength(5)]
        public string User_Per_User { get; set; }

        [StringLength(5)]
        public string User_Per_Product { get; set; }

        [StringLength(5)]
        public string User_Per_Currency { get; set; }

        [StringLength(5)]
        public string User_Per_Data { get; set; }

        [StringLength(5)]
        public string User_OfflineAccessAllowed { get; set; }

        [StringLength(50)]
        public string user_MobileEmail { get; set; }

        [StringLength(40)]
        public string user_per_tree { get; set; }

        public int? user_rollupto { get; set; }

        [StringLength(40)]
        public string user_forecastcurrency { get; set; }

        [StringLength(5)]
        public string user_per_customise { get; set; }

        public int? User_MinMemory { get; set; }

        public int? User_MaxMemory { get; set; }

        [StringLength(40)]
        public string User_ForecastSecurity { get; set; }

        [StringLength(50)]
        public string user_title { get; set; }

        [StringLength(40)]
        public string user_location { get; set; }

        [StringLength(12)]
        public string user_deskid { get; set; }

        public string User_Per_InfoAdmin { get; set; }

        [StringLength(20)]
        public string User_Device_MachineName { get; set; }

        [StringLength(40)]
        public string User_Per_Solutions { get; set; }

        [StringLength(1)]
        public string User_IsTemplate { get; set; }

        [StringLength(60)]
        public string User_TemplateName { get; set; }

        [StringLength(5)]
        public string User_WebServiceEnabled { get; set; }

        public int? user_MasterPersID { get; set; }

        public DateTime? User_AccountLockedOut { get; set; }

        [StringLength(1)]
        public string User_CTIEnabled { get; set; }

        [StringLength(40)]
        public string User_CTICallScreen { get; set; }

        [StringLength(1)]
        public string user_GroupAccess { get; set; }

        [StringLength(20)]
        public string User_LastServer { get; set; }

        [StringLength(40)]
        public string user_EnableDoNotReprice { get; set; }

        public int? User_Logons { get; set; }

        [StringLength(1)]
        public string User_ShowSurvey { get; set; }

        public DateTime? User_LastSurveyTaken { get; set; }

        [StringLength(40)]
        public string user_licencetype { get; set; }

        public int? User_TerritoriesFK { get; set; }

        public int? User_DefaultLandingPage { get; set; }

        [StringLength(40)]
        public string User_ExchangeSync { get; set; }

        [StringLength(10)]
        public string User_ShowLogs { get; set; }

        public int? User_EmarketingUserID { get; set; }

        public int? User_ExConflictCount { get; set; }

        public int? User_ExErrorCount { get; set; }

        public int? User_ExSkippedItemsCount { get; set; }

        public DateTime? User_ExLastSyncTime { get; set; }

        public int? user_assistanteid { get; set; }

        public int? user_responsableid { get; set; }

        [StringLength(40)]
        public string user_gender { get; set; }

        [StringLength(40)]
        public string user_salutation { get; set; }

        [StringLength(40)]
        public string user_titlecode { get; set; }

        [StringLength(1)]
        public string user_mergedir { get; set; }

        [StringLength(1)]
        public string user_usemergedir { get; set; }

        [StringLength(15)]
        public string user_MCpref { get; set; }

        public int? user_representantid { get; set; }

        [StringLength(40)]
        public string user_TalendExterKey { get; set; }

        [StringLength(40)]
        public string user_merge_idfield { get; set; }

        [StringLength(100)]
        public string user_merge_currentuser { get; set; }

        [StringLength(100)]
        public string user_merge_currentuserassist { get; set; }

        [StringLength(100)]
        public string user_merge_currentusermanager { get; set; }

        [StringLength(100)]
        public string user_merge_address { get; set; }

        [StringLength(100)]
        public string user_merge_1 { get; set; }

        [StringLength(100)]
        public string user_merge_2 { get; set; }

        [StringLength(100)]
        public string user_merge_3 { get; set; }

        [StringLength(100)]
        public string user_merge_4 { get; set; }

        [StringLength(100)]
        public string user_merge_5 { get; set; }

        [StringLength(100)]
        public string user_merge_6 { get; set; }

        public int? user_Per_License { get; set; }

        [StringLength(1)]
        public string user_MailMergeToWord { get; set; }

        [StringLength(1)]
        public string user_EnableMailChimp { get; set; }

        [StringLength(255)]
        public string user_flagparam { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? user_datepepga { get; set; }

        [StringLength(1)]
        public string user_nolinktoS100Etenduebt { get; set; }

        [StringLength(1)]
        public string user_nostoplinkgescom { get; set; }

        [StringLength(1)]
        public string user_norefreshca { get; set; }

        [StringLength(1)]
        public string user_nocreateaccountbt { get; set; }

        [StringLength(1)]
        public string user_nobreacklinkbt { get; set; }

        [StringLength(1)]
        public string user_noaffectlinkbt { get; set; }

        [StringLength(1)]
        public string user_ssdi_lock { get; set; }

        [StringLength(4)]
        public string user_ssdi_num_poste { get; set; }

        [StringLength(20)]
        public string user_ssdi_login { get; set; }

        [StringLength(20)]
        public string user_ssdi_password { get; set; }

        [StringLength(1)]
        public string user_ssdi_actif { get; set; }

        [StringLength(15)]
        public string user_ssdi_host_callout { get; set; }

        [StringLength(5)]
        public string user_ssdi_port_callout { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? user_ssdi_chm { get; set; }

        [StringLength(120)]
        public string user_pwdmail { get; set; }

        public string user_modelmailclient { get; set; }

        [StringLength(250)]
        public string user_destinatairesmail { get; set; }

        public string user_destinatairesmail2 { get; set; }

        [StringLength(40)]
        public string user_deptsoft { get; set; }

        public string user_Mailvalidation { get; set; }

        //public ModRech Reach { get; set; }

        public string user_mailhistorique { get; set; }

        [StringLength(50)]
        public string user_AddMac { get; set; }
    }
}
