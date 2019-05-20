using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.EntityService
{
    public class QuoteSubjectService
    {
        public string Subject { get; set; }

        private int tenant;
        private QuotePM entityPM;
        private bool isInlandDomestic { get; set; }
        private ICommonDataContext iCommonContext;
        private PortRepository iPortRepository;
        private AddressRepository iAddressRepository;
        public QuoteSubjectService(QuotePM entityPM)
        {
            this.entityPM = entityPM;
            this.tenant = entityPM.Tenant;
            this.iCommonContext = CommonDataContext.GetContext(tenant);
            this.iPortRepository = new PortRepository(iCommonContext);
            this.iAddressRepository = new AddressRepository(iCommonContext);

            if (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I")
            {
                this.isInlandDomestic = true;
            }
        }

        public string GetSubject()
        {
            this.GetPickUp();

            if (this.isInlandDomestic)
            {
                this.GetFromPartner();
                this.GetToPartner();
            }

            else
            {
                this.GetFromPort();
                this.GetToPort();
            }

            this.GetDelivery();

            this.GetIncoterm();

            return this.Subject;
        }

        private void GetPickUp()
        {
            if (entityPM.IncludePickUp)
            {
                string field = null;

                if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
                {
                    Address myAddress = iAddressRepository.GetSingleAddress(entityPM.PickUpAddressId, tenant);
                    if (myAddress != null)
                    {
                        if (!string.IsNullOrEmpty(myAddress.City))
                        {
                            field = myAddress.City;
                        }

                        if (!string.IsNullOrEmpty(myAddress.ZipCode))
                        {
                            field = string.IsNullOrEmpty(field) ? myAddress.ZipCode : field + " " + myAddress.ZipCode;
                        }
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(entityPM.FromAddressCity))
                    {
                        field = entityPM.FromAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPM.FromAddressZipCode))
                    {
                        field = string.IsNullOrEmpty(field) ? entityPM.FromAddressZipCode : field + " " + entityPM.FromAddressZipCode;
                    }
                }

                this.Append(field);
            }
        }
        private void GetFromPort()
        {
            if (!string.IsNullOrEmpty(entityPM.FromPortId))
            {
                Port iPort = iPortRepository.GetSinglePort(tenant, entityPM.FromPortId);

                if (iPort != null)
                {
                    this.Append(iPort.Code);
                }
            }
        }
        private void GetFromPartner()
        {
            if (!string.IsNullOrEmpty(entityPM.FromPartnerAddressId))
            {
                Address myAddress = iAddressRepository.GetSingleAddress(entityPM.FromPartnerAddressId, tenant);
                if (myAddress != null)
                {
                    string field = null;

                    if (!string.IsNullOrEmpty(myAddress.City))
                    {
                        field = myAddress.City;
                    }

                    if (!string.IsNullOrEmpty(myAddress.ZipCode))
                    {
                        field = string.IsNullOrEmpty(field) ? myAddress.ZipCode : field + " " + myAddress.ZipCode;
                    }

                    this.Append(field);
                }
            }
        }
        private void GetToPort()
        {
            if (!string.IsNullOrEmpty(entityPM.ToPortId))
            {
                Port iPort = iPortRepository.GetSinglePort(tenant, entityPM.ToPortId);
                if (iPort != null)
                {
                    this.Append(iPort.Code);
                }
            }
        }
        private void GetToPartner()
        {
            if (!string.IsNullOrEmpty(entityPM.ToPartnerAddressId))
            {
                Address myAddress = iAddressRepository.GetSingleAddress(entityPM.ToPartnerAddressId, tenant);

                if (myAddress != null)
                {
                    string field = null;

                    if (!string.IsNullOrEmpty(myAddress.City))
                    {
                        field = myAddress.City;
                    }

                    if (!string.IsNullOrEmpty(myAddress.ZipCode))
                    {
                        field = string.IsNullOrEmpty(field) ? myAddress.ZipCode : field + " " + myAddress.ZipCode;
                    }

                    this.Append(field);
                }
            }
        }
        private void GetDelivery()
        {
            if (entityPM.IncludeDelivery)
            {
                string field = null;

                if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                {
                    Address myAddress = iAddressRepository.GetSingleAddress(entityPM.DeliveryAddressId, tenant);
                    if (myAddress != null)
                    {
                        if (!string.IsNullOrEmpty(myAddress.City))
                        {
                            field = myAddress.City;
                        }

                        if (!string.IsNullOrEmpty(myAddress.ZipCode))
                        {
                            field = string.IsNullOrEmpty(field) ? myAddress.ZipCode : field + " " + myAddress.ZipCode;
                        }
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(entityPM.ToAddressCity))
                    {
                        field = entityPM.ToAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPM.ToAddressZipCode))
                    {
                        field = string.IsNullOrEmpty(field) ? entityPM.ToAddressZipCode : field + " " + entityPM.ToAddressZipCode;
                    }
                }

                this.Append(field);
            }
        }

        private void GetIncoterm()
        {
            if (!string.IsNullOrEmpty(entityPM.IncotermId))
            {
                IncotermRepository incotermRepository = new IncotermRepository(iCommonContext);
                Incoterm incoterm = incotermRepository.GetSingleIncoterm(entityPM.IncotermId, tenant);
                if (incoterm != null)
                {
                    if (string.IsNullOrEmpty(Subject))
                    {
                        Subject = incoterm.Code;
                    }

                    else
                    {
                        Subject = incoterm.Code + " " + Subject;
                    }
                }
            }
        }
        private void Append(string field)
        {
            if (!string.IsNullOrEmpty(field))
            {
                if (string.IsNullOrEmpty(Subject))
                {
                    Subject = field;
                }

                else
                {
                    Subject += " > " + field;
                }
            }
        }
    }
}
