namespace Helpdesk
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ERPIntegrations
    {
        [Key]
        public int ERPI_IntegrationID { get; set; }

        public int? ERPI_CreatedBy { get; set; }

        public DateTime? ERPI_CreatedDate { get; set; }

        public int? ERPI_UpdatedBy { get; set; }

        public DateTime? ERPI_UpdatedDate { get; set; }

        public DateTime? ERPI_TimeStamp { get; set; }

        public int? ERPI_Deleted { get; set; }

        [StringLength(60)]
        public string ERPI_IntegrationName { get; set; }

        [StringLength(50)]
        public string ERPI_ProductName { get; set; }

        [StringLength(3)]
        public string ERPI_SyncEnabled { get; set; }

        public DateTime? ERPI_LastSyncTime { get; set; }

        [StringLength(10)]
        public string ERPI_SyncStatus { get; set; }

        public string ERPI_SyncSchema { get; set; }

        public string ERPI_RTDSchema { get; set; }

        public DateTime? ERPI_NextSyncTime { get; set; }

        [StringLength(1)]
        public string ERPI_CreateNewComp { get; set; }

        public int? ERPI_TerritoryID { get; set; }

        public int? ERPI_AccountManager { get; set; }

        [StringLength(20)]
        public string ERPI_AllowCreateAcc { get; set; }

        [StringLength(1)]
        public string ERPI_AllowDeleteAcc { get; set; }

        public int? ERPI_SyncInterval { get; set; }

        [StringLength(400)]
        public string ERPI_ERPURL { get; set; }

        [StringLength(40)]
        public string ERPI_ERPUser { get; set; }

        [StringLength(150)]
        public string ERPI_ERPPassword { get; set; }

        public int? ERPI_CRMUser { get; set; }

        [StringLength(150)]
        public string ERPI_CRMPassword { get; set; }

        [StringLength(200)]
        public string ERPI_CompanyName { get; set; }

        [StringLength(10)]
        public string ERPI_SchemaVersion { get; set; }

        [StringLength(10)]
        public string ERPI_MetaDataVersion { get; set; }

        public int? ERPI_MaxNumErrors { get; set; }

        [StringLength(1)]
        public string ERPI_PriceProdInMultiCurrency { get; set; }

        [StringLength(20)]
        public string ERPI_ContractVersion { get; set; }

        public int? ERPI_LastSyncConflicts { get; set; }

        public int? ERPI_TimeOut { get; set; }

        [StringLength(40)]
        public string erpi_gestcodecomptable { get; set; }

        [StringLength(4)]
        public string erpi_sufixcodecomptable { get; set; }

        [StringLength(1)]
        public string erpi_autoresponsable { get; set; }

        [StringLength(1)]
        public string erpi_nosyncproduct { get; set; }

        [StringLength(1)]
        public string erpi_etendueenablelink { get; set; }

        [StringLength(80)]
        public string erpi_etendueservername { get; set; }

        [StringLength(20)]
        public string erpi_etenduedbname { get; set; }

        public int? erpi_sectfourni { get; set; }

        [StringLength(128)]
        public string erpi_etenduetesturl { get; set; }

        [StringLength(1)]
        public string erpi_autorepresentant { get; set; }

        [StringLength(1)]
        public string erpi_etendueorder { get; set; }

        [StringLength(40)]
        public string erpi_resynchallinterval { get; set; }

        public int? erpi_port { get; set; }

        [StringLength(1)]
        public string erpi_syncsommeil { get; set; }

        [StringLength(1)]
        public string erpi_synccontact { get; set; }

        [StringLength(1)]
        public string erpi_synctel { get; set; }

        [StringLength(1)]
        public string erpi_syncaddr { get; set; }
    }
}
