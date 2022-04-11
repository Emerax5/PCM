using PCM.DTO.DTOModels;
using PCM.Data.CRUDs;
using System;
using System.Collections.Generic;
using System.Text;
using PCM.Data.Models;
using MongoDB.Bson;

namespace PCM.Core.Patients
{
    public class PaymentServices
    {
        string Collection = "Payments";
        string appDataBase = "AppDataBase";
        string TrackerTable = "InvoiceTracker";
        string CancelledPaymentTable= "CancelledPayments";

        public void AddPayment(DTO.DTOModels.Payment NewPayment)
        {

            PaymentCRUD db = new PaymentCRUD(appDataBase);
            db.InsertPayment(Collection, NewPayment);

        }
        public void SaveCancelledPayment(DTO.DTOModels.Payment NewPayment)
        {

            PaymentCRUD db = new PaymentCRUD(appDataBase);
            db.InsertPayment(CancelledPaymentTable, NewPayment);

        }

        public DTO.DTOModels.Payment GetPaymentByAppointmentId(string Id) 
        {
            PaymentCRUD db = new PaymentCRUD(appDataBase);

            return db.LoadPaymentByAppointmentId<DTO.DTOModels.Payment>(Collection,Id);

        }
        public void DeletePaymentById(ObjectId Id) 
        {

            PaymentCRUD db = new PaymentCRUD(appDataBase);
            db.DeletePaymentById<DTO.DTOModels.Payment>(Collection, Id);

        }
        public DTO.DTOModels.Payment LoadPaymentById(ObjectId Id) 
        {

            PaymentCRUD db = new PaymentCRUD(appDataBase);

            return db.LoadPaymentById<DTO.DTOModels.Payment>(Collection,Id);
        }

        public bool IsAppointmentPaid(string Id) 
        {

            PaymentCRUD db = new PaymentCRUD(appDataBase);

            return db.IsAppointmentPaid(Collection, Id);

        }

        public string GenerateInvoiceNumber() 
        {
            var tracker = new InvoiceNumberTracker();
            PaymentCRUD db = new PaymentCRUD(appDataBase);

            if (db.CheckForTracker(TrackerTable) == 0)
            {

                tracker.InvoiceNumber = 0;
                tracker.LastInvoiceDate = DateTime.Today;

            }
            else
            {

                tracker = db.LoadInvoiceTracker<InvoiceNumberTracker>(TrackerTable);

            }


            if (tracker.LastInvoiceDate < DateTime.Today)
            {
                tracker.InvoiceNumber = 0;

                tracker.LastInvoiceDate = DateTime.Today;
            }

            tracker.InvoiceNumber++;

            int invoiceNumber = tracker.InvoiceNumber;
            int  maxWidth = 3;
            DateTime theDate = DateTime.Now;
            string formattedValue = theDate.ToString("ddMMyyyy") + invoiceNumber.ToString().PadLeft(maxWidth, '0');

            db.UpsertInvoiceTracker<InvoiceNumberTracker>(TrackerTable, tracker);

            return formattedValue;
        }

        public List<DTO.DTOModels.Payment> LoadReport(DateTime MinDate, DateTime MaxDate, string Insurance) 
        {


            PaymentCRUD db = new PaymentCRUD(appDataBase);

            var dbReportList = db.PaymentReport(MinDate, MaxDate, Insurance, Collection);

            List<DTO.DTOModels.Payment> reportList = new List<DTO.DTOModels.Payment>();

            foreach (var payment in dbReportList)
            {
                DTO.DTOModels.Payment reportPayment = new DTO.DTOModels.Payment();

                reportPayment.AppointmentDate = payment.AppointmentDate;
                reportPayment.AppointmentId = payment.AppointmentId;
                reportPayment.AppointmentPrice = payment.AppointmentPrice;
                reportPayment.Description = payment.Description;
                reportPayment.EmitedBy = payment.EmitedBy;
                reportPayment.EmitedTime = payment.EmitedTime;
                reportPayment.Id = payment.Id;
                reportPayment.Insurance = payment.Insurance;
                reportPayment.InsuranceCardnet = payment.InsuranceCardnet;
                reportPayment.InsuranceCoverage = payment.InsuranceCoverage;
                reportPayment.PatientId = payment.PatientId;
                reportPayment.PatientName = payment.PatientName;
                reportPayment.PatientPhone = payment.PatientPhone;
                reportPayment.ReceiptNumber = payment.ReceiptNumber;
                reportPayment.TotalPayment = payment.TotalPayment;

                reportList.Add(reportPayment);

            }


            return reportList;          

        }
       
    }
}
