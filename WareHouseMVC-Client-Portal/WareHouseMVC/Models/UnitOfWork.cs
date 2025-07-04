using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WareHouseMVC.Models
{
    public class UnitOfWork : IDisposable
    {

        private WareHouseMVCContext context;
        public UnitOfWork()
        {
            context = new WareHouseMVCContext();
        }
        public UnitOfWork(WareHouseMVCContext _context)
        {
            this.context = _context;
        }

      

        private UserRepository _userRepository;

        public UserRepository UserRepository
        {
            get
            {

                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(context);
                }
                return _userRepository;
            }
        }


     

        private DelPendingBoxModelRepository _delPendingBoxModelRepository;

        public DelPendingBoxModelRepository DelPendingBoxModelRepository
        {
            get
            {

                if (this._delPendingBoxModelRepository == null)
                {
                    this._delPendingBoxModelRepository = new DelPendingBoxModelRepository(context);
                }
                return _delPendingBoxModelRepository;
            }
        }


        private DelPendingBoxModelFileRepository _delPendingBoxModelFileRepository;

        public DelPendingBoxModelFileRepository DelPendingBoxModelFileRepository
        {
            get
            {

                if (this._delPendingBoxModelFileRepository == null)
                {
                    this._delPendingBoxModelFileRepository = new DelPendingBoxModelFileRepository(context);
                }
                return _delPendingBoxModelFileRepository;
            }
        }


        private ErrorLogRepository _errorLogRepository;

        public ErrorLogRepository ErrorLogRepository
        {
            get
            {

                if (this._errorLogRepository == null)
                {
                    this._errorLogRepository = new ErrorLogRepository(context);
                }
                return _errorLogRepository;
            }
        }


        private BoxDestructionRepository _boxDestructionRepository;

        public BoxDestructionRepository BoxDestructionRepository
        {
            get
            {

                if (this._boxDestructionRepository == null)
                {
                    this._boxDestructionRepository = new BoxDestructionRepository(context);
                }
                return _boxDestructionRepository;
            }
        }


        private LoginTrailRepository _loginTrailRepository;

        public LoginTrailRepository LoginTrailRepository
        {
            get
            {

                if (this._loginTrailRepository == null)
                {
                    this._loginTrailRepository = new LoginTrailRepository(context);
                }
                return _loginTrailRepository;
            }
        }


        private BarcodeMappingRepository _barcodeMappingRepository;

        public BarcodeMappingRepository BarcodeMappingRepository
        {
            get
            {

                if (this._barcodeMappingRepository == null)
                {
                    this._barcodeMappingRepository = new BarcodeMappingRepository(context);
                }
                return _barcodeMappingRepository;
            }
        }


        private TransmittalINAuditTrailRepository _transmittalINAuditTrailRepository;

        public TransmittalINAuditTrailRepository TransmittalINAuditTrailRepository
        {
            get
            {

                if (this._transmittalINAuditTrailRepository == null)
                {
                    this._transmittalINAuditTrailRepository = new TransmittalINAuditTrailRepository(context);
                }
                return _transmittalINAuditTrailRepository;
            }
        }


        private TransmittalOUTAuditTrailRepository _transmittalOUTAuditTrailRepository;

        public TransmittalOUTAuditTrailRepository TransmittalOUTAuditTrailRepository
        {
            get
            {

                if (this._transmittalOUTAuditTrailRepository == null)
                {
                    this._transmittalOUTAuditTrailRepository = new TransmittalOUTAuditTrailRepository(context);
                }
                return _transmittalOUTAuditTrailRepository;
            }
        }
        

        private PalletRepository _palletRepository;

        public PalletRepository PalletRepository
        {
            get
            {

                if (this._palletRepository == null)
                {
                    this._palletRepository = new PalletRepository(context);
                }
                return _palletRepository;
            }
        }


        private AutoZoneSuggentionRepository _autoZoneSuggentionRepository;

        public AutoZoneSuggentionRepository AutoZoneSuggentionRepository
        {
            get
            {

                if (this._autoZoneSuggentionRepository == null)
                {
                    this._autoZoneSuggentionRepository = new AutoZoneSuggentionRepository(context);
                }
                return _autoZoneSuggentionRepository;
            }
        }

        private ChangeLocationRepository _changeLocationRepository;

        public ChangeLocationRepository ChangeLocationRepository
        {
            get
            {

                if (this._changeLocationRepository == null)
                {
                    this._changeLocationRepository = new ChangeLocationRepository(context);
                }
                return _changeLocationRepository;
            }
        }



        private BoxOutStatementViewModelRepository _oxOutStatementViewModelRepository;

        public BoxOutStatementViewModelRepository BoxOutStatementViewModelRepository
        {
            get
            {

                if (this._oxOutStatementViewModelRepository == null)
                {
                    this._oxOutStatementViewModelRepository = new BoxOutStatementViewModelRepository(context);
                }
                return _oxOutStatementViewModelRepository;
            }
        }



        private BoxSheetRepository _boxSheetRepository;

        public BoxSheetRepository BoxSheetRepository
        {
            get
            {

                if (this._boxSheetRepository == null)
                {
                    this._boxSheetRepository = new BoxSheetRepository(context);
                }
                return _boxSheetRepository;
            }
        }


        private ORBLOperatorRepository _oRBLOperatorRepository;

        public ORBLOperatorRepository ORBLOperatorRepository
        {
            get
            {

                if (this._oRBLOperatorRepository == null)
                {
                    this._oRBLOperatorRepository = new ORBLOperatorRepository(context);
                }
                return _oRBLOperatorRepository;
            }
        }


        private BoxINStatementViewModelRepository _boxINStatementViewModelRepository;

        public BoxINStatementViewModelRepository BoxINStatementViewModelRepository
        {
            get
            {

                if (this._boxINStatementViewModelRepository == null)
                {
                    this._boxINStatementViewModelRepository = new BoxINStatementViewModelRepository(context);
                }
                return _boxINStatementViewModelRepository;
            }
        }


        private HostInformationRepository _hostInformationRepository;

        public HostInformationRepository HostInformationRepository
        {
            get
            {

                if (this._hostInformationRepository == null)
                {
                    this._hostInformationRepository = new HostInformationRepository(context);
                }
                return _hostInformationRepository;
            }
        }



        private EmptyBoxRepository _emptyBoxRepository;

        public EmptyBoxRepository EmptyBoxRepository
        {
            get
            {

                if (this._emptyBoxRepository == null)
                {
                    this._emptyBoxRepository = new EmptyBoxRepository(context);
                }
                return _emptyBoxRepository;
            }
        }


        private TrainRepository _trainRepository;

        public TrainRepository TrainRepository
        {
            get
            {

                if (this._trainRepository == null)
                {
                    this._trainRepository = new TrainRepository(context);
                }
                return _trainRepository;
            }
        }



        private BarCodeRepository _barCodeRepository;

        public BarCodeRepository BarCodeRepository
        {
            get
            {

                if (this._barCodeRepository == null)
                {
                    this._barCodeRepository = new BarCodeRepository(context);
                }
                return _barCodeRepository;
            }
        }
      


        private AssignBoxRepository _assignBoxRepository;

        public AssignBoxRepository AssignBoxRepository
        {
            get
            {

                if (this._assignBoxRepository == null)
                {
                    this._assignBoxRepository = new AssignBoxRepository(context);
                }
                return _assignBoxRepository;
            }
        }



        private AssignBoxTrOUTRepository _assignBoxTrOUTRepository;

        public AssignBoxTrOUTRepository AssignBoxTrOUTRepository
        {
            get
            {

                if (this._assignBoxTrOUTRepository == null)
                {
                    this._assignBoxTrOUTRepository = new AssignBoxTrOUTRepository(context);
                }
                return _assignBoxTrOUTRepository;
            }
        }


        private TransmittalOUTRepository _transmittalOUTRepository;

        public TransmittalOUTRepository TransmittalOUTRepository
        {
            get
            {

                if (this._transmittalOUTRepository == null)
                {
                    this._transmittalOUTRepository = new TransmittalOUTRepository(context);
                }
                return _transmittalOUTRepository;
            }
        }

        private TransmittalOUTStatusRepository _transmittalOUTStatusRepository;

        public TransmittalOUTStatusRepository TransmittalOUTStatusRepository
        {
            get
            {

                if (this._transmittalOUTStatusRepository == null)
                {
                    this._transmittalOUTStatusRepository = new TransmittalOUTStatusRepository(context);
                }
                return _transmittalOUTStatusRepository;
            }
        }



        private BoxLocationRepository _boxLocationRepository;

        public BoxLocationRepository BoxLocationRepository
        {
            get
            {

                if (this._boxLocationRepository == null)
                {
                    this._boxLocationRepository = new BoxLocationRepository(context);
                }
                return _boxLocationRepository;
            }
        }


        private ProjectRepository _projectRepository;

        public ProjectRepository ProjectRepository
        {
            get
            {

                if (this._projectRepository == null)
                {
                    this._projectRepository = new ProjectRepository(context);
                }
                return _projectRepository;
            }
        }



        private HandOverByRepository _handOverByRepository;

        public HandOverByRepository HandOverByRepository
        {
            get
            {

                if (this._handOverByRepository == null)
                {
                    this._handOverByRepository = new HandOverByRepository(context);
                }
                return _handOverByRepository;
            }
        }


        private TransmittalINStatusRepository _transmittalINStatusRepository;

        public TransmittalINStatusRepository TransmittalINStatusRepository
        {
            get
            {

                if (this._transmittalINStatusRepository == null)
                {
                    this._transmittalINStatusRepository = new TransmittalINStatusRepository(context);
                }
                return _transmittalINStatusRepository;
            }
        }

        private ReceivedByRepository _receivedByRepository;

        public ReceivedByRepository ReceivedByRepository
        {
            get
            {

                if (this._receivedByRepository == null)
                {
                    this._receivedByRepository = new ReceivedByRepository(context);
                }
                return _receivedByRepository;
            }
        }


        private ChallanINRepository _challanINRepository;

        public ChallanINRepository ChallanINRepository
        {
            get
            {

                if (this._challanINRepository == null)
                {
                    this._challanINRepository = new ChallanINRepository(context);
                }
                return _challanINRepository;
            }
        }


        private RackRepository _rackRepository;

        public RackRepository RackRepository
        {
            get
            {

                if (this._rackRepository == null)
                {
                    this._rackRepository = new RackRepository(context);
                }
                return _rackRepository;
            }
        }


        private HeightRepository _heightRepository;

        public HeightRepository HeightRepository
        {
            get
            {

                if (this._heightRepository == null)
                {
                    this._heightRepository = new HeightRepository(context);
                }
                return _heightRepository;
            }
        }



        private ColumnRepository _columnRepository;

        public ColumnRepository ColumnRepository
        {
            get
            {

                if (this._columnRepository == null)
                {
                    this._columnRepository = new ColumnRepository(context);
                }
                return _columnRepository;
            }
        }


        private RowRepository _rowRepository;

        public RowRepository RowRepository
        {
            get
            {

                if (this._rowRepository == null)
                {
                    this._rowRepository = new RowRepository(context);
                }
                return _rowRepository;
            }
        }


        private ClientRepository _clientRepository;

        public ClientRepository ClientRepository
        {
            get
            {

                if (this._clientRepository == null)
                {
                    this._clientRepository = new ClientRepository(context);
                }
                return _clientRepository;
            }
        }



        private ItemRepository _itemRepository;

        public ItemRepository ItemRepository
        {
            get
            {

                if (this._itemRepository == null)
                {
                    this._itemRepository = new ItemRepository(context);
                }
                return _itemRepository;
            }
        }





        private DepartmentRepository _departmentRepository;

        public DepartmentRepository DepartmentRepository
        {
            get
            {

                if (this._departmentRepository == null)
                {
                    this._departmentRepository = new DepartmentRepository(context);
                }
                return _departmentRepository;
            }
        }



        private ContactPersonRepository _contactPersonRepository;

        public ContactPersonRepository ContactPersonRepository
        {
            get
            {

                if (this._contactPersonRepository == null)
                {
                    this._contactPersonRepository = new ContactPersonRepository(context);
                }
                return _contactPersonRepository;
            }
        }


        private ORBLDepartmentRepository _oRBLDepartmentRepository;

        public ORBLDepartmentRepository ORBLDepartmentRepository
        {
            get
            {

                if (this._oRBLDepartmentRepository == null)
                {
                    this._oRBLDepartmentRepository = new ORBLDepartmentRepository(context);
                }
                return _oRBLDepartmentRepository;
            }
        }


        private SupportStuffRepository _supportStuffRepository;

        public SupportStuffRepository SupportStuffRepository
        {
            get
            {

                if (this._supportStuffRepository == null)
                {
                    this._supportStuffRepository = new SupportStuffRepository(context);
                }
                return _supportStuffRepository;
            }
        }


        private DriverRepository _driverRepository;

        public DriverRepository DriverRepository
        {
            get
            {

                if (this._driverRepository == null)
                {
                    this._driverRepository = new DriverRepository(context);
                }
                return _driverRepository;
            }
        }




        private TransportVendorRepository _transportVendorRepository;

        public TransportVendorRepository TransportVendorRepository
        {
            get
            {

                if (this._transportVendorRepository == null)
                {
                    this._transportVendorRepository = new TransportVendorRepository(context);
                }
                return _transportVendorRepository;
            }
        }


        private VehicleRepository _vehicleRepository;

        public VehicleRepository VehicleRepository
        {
            get
            {

                if (this._vehicleRepository == null)
                {
                    this._vehicleRepository = new VehicleRepository(context);
                }
                return _vehicleRepository;
            }
        }


        private LevelRepository _levelRepository;

        public LevelRepository LevelRepository
        {
            get
            {

                if (this._levelRepository == null)
                {
                    this._levelRepository = new LevelRepository(context);
                }
                return _levelRepository;
            }
        }


        private AuditTrailCPRepository _auditTrailCPRepository;

        public AuditTrailCPRepository AuditTrailCPRepository
        {
            get
            {

                if (this._auditTrailCPRepository == null)
                {
                    this._auditTrailCPRepository = new AuditTrailCPRepository(context);
                }
                return _auditTrailCPRepository;
                

            }
        }



        private TransmittalINRepository _transmittalINRepository;

        public TransmittalINRepository TransmittalINRepository
        {
            get
            {

                if (this._transmittalINRepository == null)
                {
                    this._transmittalINRepository = new TransmittalINRepository(context);
                }
                return _transmittalINRepository;
            }
        }


        private RoleRepository _roleRepository;

        public RoleRepository RoleRepository
        {
            get
            {

                if (this._roleRepository == null)
                {
                    this._roleRepository = new RoleRepository(context);
                }
                return _roleRepository;
            }
        }


        private WarehouseRepository _warehouseRepository;

        public WarehouseRepository WarehouseRepository
        {
            get
            {

                if (this._warehouseRepository == null)
                {
                    this._warehouseRepository = new WarehouseRepository(context);
                }
                return _warehouseRepository;
            }
        }

        private FloorRepository _floorRepository;

        public FloorRepository FloorRepository
        {
            get
            {

                if (this._floorRepository == null)
                {
                    this._floorRepository = new FloorRepository(context);
                }
                return _floorRepository;
            }
        }

        private ZoneRepository _zoneRepository;

        public ZoneRepository ZoneRepository
        {
            get
            {

                if (this._zoneRepository == null)
                {
                    this._zoneRepository = new ZoneRepository(context);
                }
                return _zoneRepository;
            }
        }


        private RegionRepository _regionRepository;

        public RegionRepository RegionRepository
        {
            get
            {

                if (this._regionRepository == null)
                {
                    this._regionRepository = new RegionRepository(context);
                }
                return _regionRepository;
            }
        }

        private ClientBillingInfoRepository _clientBillingInfoRepository;

        public ClientBillingInfoRepository ClientBillingInfoRepository
        {
            get
            {

                if (this._clientBillingInfoRepository == null)
                {
                    this._clientBillingInfoRepository = new ClientBillingInfoRepository(context);
                }
                return _clientBillingInfoRepository;
            }
        }


        private InvoiceViewModelRepository _invoiceViewModelRepository;

        public InvoiceViewModelRepository InvoiceViewModelRepository
        {
            get
            {

                if (this._invoiceViewModelRepository == null)
                {
                    this._invoiceViewModelRepository = new InvoiceViewModelRepository(context);
                }
                return _invoiceViewModelRepository;
            }
        }



        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}