namespace Helpdesk
{
    using System.Data.Entity;

    public partial class ModelHELPD : DbContext
    {
        public ModelHELPD()
            : base("name=ModelHELPD")
        {
        }

        public virtual DbSet<Cases> Cases { get; set; }
        public virtual DbSet<Comm_Link> Comm_Link { get; set; }
        public virtual DbSet<Communication> Communication { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Crmcli_CR> Crmcli_CR { get; set; }
        public virtual DbSet<Crmcli_Demandes> Crmcli_Demandes { get; set; }
        public virtual DbSet<Crmcli_EnvoiMails> Crmcli_EnvoiMails { get; set; }
        public virtual DbSet<Crmcli_Reg> Crmcli_Reg { get; set; }
        public virtual DbSet<Crmcli_SousTaches> Crmcli_SousTaches { get; set; }
        public virtual DbSet<Crmcli_Taches> Crmcli_Taches { get; set; }
        public virtual DbSet<Custom_Captions> Custom_Captions { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PLANDACTION> PLANDACTION { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<vPerson> vPerson { get; set; }
        public virtual DbSet<Crmcli_TickDem> Crmcli_TickDem { get; set; }

        public virtual DbSet<Crmcli_HistoIntervs> Crmcli_HistoIntervs { get; set; }

        public virtual DbSet<Crmcli_NumFiche> Crmcli_NumFiche { get; set; }
        public virtual DbSet<Crmcli_UsersSession> Crmcli_UsersSession { get; set; }

        public virtual DbSet<Crmcli_Administrateurs> Crmcli_Administrateurs { get; set; }
        public virtual DbSet<Crmcli_AffectationProds> Crmcli_AffectationProds { get; set; }
        public virtual DbSet<Crmcli_ValidateFiches> Crmcli_ValidateFiches { get; set; }
        public virtual DbSet<Crmcli_Signatures> Crmcli_Signatures { get; set; }

        public virtual DbSet<Crmcli_Quests> Crmcli_Quests { get; set; }

        public virtual DbSet<Crmcli_HistoriqueRelances> Crmcli_HistoriqueRelances { get; set; }

        public virtual DbSet<Crmcom_AnalBesoinProsp> Crmcom_AnalBesoinProsp { get; set; }
        public virtual DbSet<Crmcom_AutrRDVProsp> Crmcom_AutrRDVProsp { get; set; }
        public virtual DbSet<Crmcom_AutrRDVvProsp> Crmcom_AutrRDVvProsp { get; set; }
        public virtual DbSet<Crmcom_BPAProsp> Crmcom_BPAProsp { get; set; }
        public virtual DbSet<Crmcom_BPARELProsp> Crmcom_BPARELProsp { get; set; }
        public virtual DbSet<Crmcom_ClassifProsp> Crmcom_ClassifProsp { get; set; }
        public virtual DbSet<Crmcom_CommercialeProsp> Crmcom_CommercialeProsp { get; set; }
        public virtual DbSet<Crmcom_DemoMaqProsp> Crmcom_DemoMaqProsp { get; set; }
        public virtual DbSet<Crmcom_DemoProsp> Crmcom_DemoProsp { get; set; }
        public virtual DbSet<Crmcom_DevisGescomProsp> Crmcom_DevisGescomProsp { get; set; }
        public virtual DbSet<Crmcom_EtatProsp> Crmcom_EtatProsp { get; set; }
        public virtual DbSet<Crmcom_FAProsp> Crmcom_FAProsp { get; set; }
        public virtual DbSet<Crmcom_NegoProsp> Crmcom_NegoProsp { get; set; }
        public virtual DbSet<Crmcom_RebutProsp> Crmcom_RebutProsp { get; set; }
        public virtual DbSet<Crmcom_RebutReaProsp> Crmcom_RebutReaProsp { get; set; }
        public virtual DbSet<Crmcom_RemiDevisProsp> Crmcom_RemiDevisProsp { get; set; }
        //public virtual DbSet<Crmcom_SourceProsp> Crmcom_SourceProsp { get; set; }
        public virtual DbSet<Crmcom_TypeCProsp> Crmcom_TypeCProsp { get; set; }

        public virtual DbSet<Crmcli_Theme> Crmcli_Theme { get; set; }

        public virtual DbSet<ERPIntegrations> ERPIntegrations { get; set; }

        public virtual DbSet<Opportunity> Opportunity { get; set; }

        public virtual DbSet<SSDI_BUSINESS> SSDI_BUSINESS { get; set; }
        public virtual DbSet<SSDI_SERVICE> SSDI_SERVICE { get; set; }

        public virtual DbSet<Crmcli_CONVENTIONS> Crmcli_CONVENTIONS { get; set; }

        public virtual DbSet<Campaigns> Campaigns { get; set; }
        public virtual DbSet<WaveItems> WaveItems { get; set; }
        public virtual DbSet<Waves> Waves { get; set; }

        public virtual DbSet<Currency> Currency { get; set; }

        public virtual DbSet<pro_planning> pro_planning { get; set; }
        public virtual DbSet<pro_rapport> pro_rapport { get; set; }
        public virtual DbSet<pro_tache> pro_tache { get; set; }
        public virtual DbSet<pro_user> pro_user { get; set; }

        public virtual DbSet<Crmcli_SUPSUBo> Crmcli_SUPSUBo { get; set; }
        public virtual DbSet<Crmcli_SUPSUBv> Crmcli_SUPSUBv { get; set; }

        public virtual DbSet<ThistoSendMail> ThistoSendMail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_nomail)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_visibleExtranetSig)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_totalValuation)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_totalCommTime)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_JP)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_tpsvendu)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_resteafaire)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_ElapsedTimeSolved)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_ElapsedTimeLogged)
                .HasPrecision(7, 2);

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_ssdi_nonvisibltech)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_gamme)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_Etape)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_djFA)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_oncreatesendsms)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_onclosesendsms)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_p1)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_p3)
                .IsFixedLength();

            modelBuilder.Entity<Cases>()
                .Property(e => e.case_p4)
                .IsFixedLength();

            modelBuilder.Entity<Comm_Link>()
                .Property(e => e.CmLi_Comm_InitialWave)
                .IsFixedLength();

            modelBuilder.Entity<Comm_Link>()
                .Property(e => e.Cmli_SMSMessageSent)
                .IsFixedLength();

            modelBuilder.Entity<Comm_Link>()
                .Property(e => e.Cmli_MassEmailSent)
                .IsFixedLength();

            modelBuilder.Entity<Comm_Link>()
                .Property(e => e.cmli_reminder)
                .IsFixedLength();

            modelBuilder.Entity<Comm_Link>()
                .Property(e => e.CmLi_IsExternalAttendee)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_Private)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_SMSMessageSent)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_SMSNotification)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_IsHtml)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_HasAttachments)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_EmailLinksCreated)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_taskreminder)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_CRMOnly)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_Exception)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_IsAllDayEvent)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.Comm_IsStub)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_temps)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_deptemps)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_valorisqte)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_depvalorisqte)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_valoriscalc)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_depcalc)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_closeCase)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_dureeact)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ESSAITCOMPTE)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_delai_traitement)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_visibleExtranetSig)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_ssdi_nonvisibltech)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_delaitraitement)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_sendconfirm)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_sendreminder)
                .IsFixedLength();

            modelBuilder.Entity<Communication>()
                .Property(e => e.comm_obfuscated)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_promote)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.Comp_OptOut)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_gdalangue)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_public)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_actionrisque_repli)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_satisfaction)
                .HasPrecision(3, 2);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_vip)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_solde)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_encaisse)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_encours1)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_encours2)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_encours3)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_ssdi_BLFA)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_capitalsoc)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_prjenc)
                .IsFixedLength();

            modelBuilder.Entity<Company>()
                .Property(e => e.comp_obfuscated)
                .IsFixedLength();

            //modelBuilder.Entity<Crmcli_Demandes>()
            //    .HasMany(e => e.Crmcli_Taches)
            //    .WithOptional(e => e.Crmcli_Demandes)
            //    .HasForeignKey(e => e.IDDemande);

            //modelBuilder.Entity<Crmcli_Produits>()
            //    .HasMany(e => e.Crmcli_Demandes)
            //    .WithOptional(e => e.Crmcli_Produits)
            //    .HasForeignKey(e => e.ProduitDemande);

            //modelBuilder.Entity<Crmcli_Taches>()
            //    .HasMany(e => e.Crmcli_SousTaches)
            //    .WithOptional(e => e.Crmcli_Taches)
            //    .HasForeignKey(e => e.ID_Taches);

            //modelBuilder.Entity<Crmcli_TypesDemandes>()
            //    .HasMany(e => e.Crmcli_Demandes)
            //    .WithOptional(e => e.Crmcli_TypesDemandes)
            //    .HasForeignKey(e => e.TypeDemande);

            modelBuilder.Entity<Crmcli_Demandes>()
                .HasMany(e => e.Crmcli_Taches)
                .WithOptional(e => e.Crmcli_Demandes)
                .HasForeignKey(e => e.IDDemande);

            modelBuilder.Entity<Custom_Captions>()
                .Property(e => e.Capt_System)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.pers_promote)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.Pers_OptOut)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.pers_ssdi_extranet)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.pers_ssdi_estcontactn2)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.pers_ssdi_anotifiern2)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.pers_decideur)
                .IsFixedLength();

            modelBuilder.Entity<Person>()
                .Property(e => e.pers_obfuscated)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_Language)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_MustChangePassword)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_CannotChangePassword)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_PasswordNeverExpires)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_SMSNotification)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_Disabled)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_OfflineAccessAllowed)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_per_customise)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_IsTemplate)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_WebServiceEnabled)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_CTIEnabled)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_GroupAccess)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.User_ShowSurvey)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_mergedir)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_usemergedir)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_MailMergeToWord)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_EnableMailChimp)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_datepepga)
                .HasPrecision(24, 6);

            modelBuilder.Entity<users>()
                .Property(e => e.user_nolinktoS100Etenduebt)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_nostoplinkgescom)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_norefreshca)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_nocreateaccountbt)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_nobreacklinkbt)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_noaffectlinkbt)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_ssdi_lock)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_ssdi_actif)
                .IsFixedLength();

            modelBuilder.Entity<users>()
                .Property(e => e.user_ssdi_chm)
                .HasPrecision(6, 2);

            modelBuilder.Entity<vPerson>()
                .Property(e => e.Pers_PhoneCountryCode)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.Pers_FaxCountryCode)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.pers_promote)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.Pers_OptOut)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.pers_ssdi_extranet)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.pers_ssdi_estcontactn2)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.pers_ssdi_anotifiern2)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.pers_decideur)
                .IsFixedLength();

            modelBuilder.Entity<vPerson>()
                .Property(e => e.pers_obfuscated)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.ERPI_SyncEnabled)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.ERPI_CreateNewComp)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.ERPI_AllowDeleteAcc)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.ERPI_PriceProdInMultiCurrency)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_sufixcodecomptable)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_autoresponsable)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_nosyncproduct)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_etendueenablelink)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_autorepresentant)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_etendueorder)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_syncsommeil)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_synccontact)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_synctel)
                .IsFixedLength();

            modelBuilder.Entity<ERPIntegrations>()
                .Property(e => e.erpi_syncaddr)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.Oppo_Forecast)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.Oppo_Total)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_TotalOrders)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_totalQuotes)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_NoDiscAmtSum)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_marge)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_margepond)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_caneg)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_canegma)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_caneglo)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_margneg)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_acnegma)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_acneglo)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_caservice)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_margser)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_proj_ct)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_vente_sage)
                .HasPrecision(12, 3);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ssdi_vente_sage_pourc)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.Oppo_SCRMIsCrossSell)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_nb_param)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_nb_jr_formation)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_nb_jr_dev)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_jr_facture)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_nb_jr_tot)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_jr_offert)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_parc)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_dernierprix)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_durreTraitement)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_ModalitPaiement)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p1)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p2)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p4)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p3)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p6)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p7)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p8)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p9)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p10)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p11)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p12)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p13)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p14)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p16)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p15)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p17)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p18)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p20)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p19)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p21)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p22)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p23)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p24)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p25)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_P26)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_p27)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_obfuscated)
                .IsFixedLength();

            modelBuilder.Entity<Opportunity>()
                .Property(e => e.oppo_NbreSal)
                .HasPrecision(24, 6);

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_prix)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_qtemodele)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_financeloyer)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_financemont)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_financeracha)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_acqlog)
                .IsFixedLength();

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_reabo)
                .IsFixedLength();

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_nonvisibltech)
                .IsFixedLength();

            modelBuilder.Entity<SSDI_BUSINESS>()
                .Property(e => e.busi_nbrejourlim)
                .HasPrecision(24, 6);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_partenairecommission)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_tarif)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_qtevendu)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_qteofferte)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_reportforfaitprec)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_equivheure)
                .HasPrecision(10, 6);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_equivheureN2)
                .HasPrecision(10, 6);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_prixunitaire)
                .HasPrecision(12, 3);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_deplacement)
                .HasPrecision(5, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_total_credit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_actions_total_conso)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_actions_pourcent_valoris)
                .HasPrecision(5, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_actions_pourcent_conso)
                .HasPrecision(7, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_total_solde)
                .HasPrecision(8, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_actions_total_tps_mo)
                .HasPrecision(6, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_actions_total_tps_depl)
                .HasPrecision(6, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_pourcent_temps_passe)
                .HasPrecision(5, 2);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_nbjr)
                .HasPrecision(24, 6);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_visibleExtranetSig)
                .IsFixedLength();

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_nonvisibltech)
                .IsFixedLength();

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_nombrejourfacture)
                .HasPrecision(24, 6);

            modelBuilder.Entity<SSDI_SERVICE>()
                .Property(e => e.serv_nombrejouroffert)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Campaigns>()
                .Property(e => e.Camp_Budget)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Campaigns>()
                .Property(e => e.Camp_Cost)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Campaigns>()
                .Property(e => e.Camp_Launched)
                .IsFixedLength();

            modelBuilder.Entity<WaveItems>()
                .Property(e => e.WaIt_Budget)
                .HasPrecision(24, 6);

            modelBuilder.Entity<WaveItems>()
                .Property(e => e.WaIt_Cost)
                .HasPrecision(24, 6);

            modelBuilder.Entity<WaveItems>()
                .Property(e => e.wait_commpriority)
                .IsFixedLength();

            modelBuilder.Entity<Waves>()
                .Property(e => e.Wave_Budget)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Waves>()
                .Property(e => e.Wave_Cost)
                .HasPrecision(24, 6);

            modelBuilder.Entity<Currency>()
                .Property(e => e.Curr_Rate)
                .HasPrecision(24, 6);
        }
    }
}
