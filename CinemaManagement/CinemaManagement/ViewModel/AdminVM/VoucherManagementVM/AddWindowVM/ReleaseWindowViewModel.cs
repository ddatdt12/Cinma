using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Utils;
using CinemaManagement.Views.Admin.VoucherManagement.AddWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace CinemaManagement.ViewModel.AdminVM.VoucherManagementVM
{
    public partial class VoucherViewModel : BaseViewModel
    {
        public static int NumberCustomer;

        private ComboBoxItem _ReleaseCustomerList;
        public ComboBoxItem ReleaseCustomerList
        {
            get { return _ReleaseCustomerList; }
            set
            {
                _ReleaseCustomerList = value;
            }
        }

        private ComboBoxItem perCus;
        public ComboBoxItem PerCus
        {
            get { return perCus; }
            set { perCus = value; OnPropertyChanged(); }
        }


        public ICommand DeleteWaitingReleaseCM { get; set; }
        public ICommand MoreEmailCM { get; set; }
        public ICommand LessEmailCM { get; set; }
        public ICommand OpenReleaseVoucherCM { get; set; }
        public ICommand ReleaseVoucherCM { get; set; }
        public ICommand ResetSelectedNumberCM { get; set; }
        public ICommand ReleaseVoucherExcelCM { get; set; }
        public ICommand CalculateNumberOfVoucherCM { get; set; }

        private ObservableCollection<VoucherDTO> releaseVoucherList;
        public ObservableCollection<VoucherDTO> ReleaseVoucherList
        {
            get { return releaseVoucherList; }
            set { releaseVoucherList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CustomerEmail> _ListCustomerEmail;
        public ObservableCollection<CustomerEmail> ListCustomerEmail
        {
            get { return _ListCustomerEmail; }
            set { _ListCustomerEmail = value; OnPropertyChanged(); }
        }

        public async Task ReleaseVoucherFunc(ReleaseVoucher p)
        {
            string mess = "Số voucher không chia hết cho khách hàng!";
            if (ReleaseVoucherList.Count == 0)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", "Danh sách voucher đang trống!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }
            foreach (var item in ListCustomerEmail)
            {
                if (string.IsNullOrEmpty(item.Email))
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Tồn tại email trống", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
            }
            //top 5 customer
            if (NumberCustomer == 5)
            {
                if (ListCustomerEmail.Count == 0)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Danh sách khách hàng đang trống!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
                else
                {
                    if (ReleaseVoucherList.Count % ListCustomerEmail.Count != 0)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", mess, MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }

            }
            // input customer mail
            else if (NumberCustomer == -1)
            {
                if (ListCustomerEmail.Count == 0)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Danh sách khách hàng đang trống!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
                if (ReleaseVoucherList.Count > ListCustomerEmail.Count)
                {
                    if (ReleaseVoucherList.Count % ListCustomerEmail.Count != 0)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", mess, MessageType.Warning, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }
                else if (ReleaseVoucherList.Count < ListCustomerEmail.Count)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", mess, MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }
            }

            // new customer
            //code here


            // Danh sách code và khách hàng
            List<string> listCode = ReleaseVoucherList.Select(v => v.Code).ToList();
            List<string> listCustomerEmail = ListCustomerEmail.Select(v => v.Email).ToList();

            //Chia danh sách code theo số lượng khách hàng
            int sizePerItem = listCode.Count / listCustomerEmail.Count;
            List<List<string>> ListCodePerEmailList = ChunkBy(listCode, sizePerItem);

            (bool sendSuccess, string messageFromSendEmail) = await sendHtmlEmail(listCustomerEmail, ListCodePerEmailList);

            if (!sendSuccess)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", messageFromSendEmail, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                return;
            }

            WaitingMiniVoucher = new List<int>();
            foreach (var item in ReleaseVoucherList)
            {
                WaitingMiniVoucher.Add(item.Id);
            }
            (bool releaseSuccess, string messageFromRelease) = await VoucherService.Ins.ReleaseMultiVoucher(WaitingMiniVoucher);

            if (releaseSuccess)
            {
                MessageBoxCustom mb = new MessageBoxCustom("", messageFromRelease, MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
                WaitingMiniVoucher.Clear();
                ReleaseVoucherList.Clear();
                try
                {
                    (VoucherReleaseDTO voucherReleaseDetail, bool haveAnyUsedVoucher) = await VoucherService.Ins.GetVoucherReleaseDetails(SelectedItem.Id);

                    SelectedItem = voucherReleaseDetail;
                    ListViewVoucher = new ObservableCollection<VoucherDTO>(SelectedItem.Vouchers);
                    StoreAllMini = new ObservableCollection<VoucherDTO>(ListViewVoucher);
                    AddVoucher.topcheck.IsChecked = false;
                    AddVoucher._cbb.SelectedIndex = 0;
                    NumberSelected = 0;
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom m = new MessageBoxCustom("", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                    m.ShowDialog();
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    MessageBoxCustom m = new MessageBoxCustom("", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                    m.ShowDialog();
                    throw;
                }

                p.Close();
            }
            else
            {
                MessageBoxCustom mb = new MessageBoxCustom("", messageFromRelease, MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
            }
        }
        public async Task RefreshEmailList()
        {
            if (ReleaseCustomerList is null) return;
            switch (ReleaseCustomerList.Content.ToString())
            {
                case "Top 5 khách hàng trong tháng":
                    {
                        List<CustomerDTO> list = await CustomerService.Ins.GetTop5CustomerEmail();
                        ListCustomerEmail = new ObservableCollection<CustomerEmail>();

                        foreach (var item in list)
                        {
                            if (item.Email != null)
                                ListCustomerEmail.Add(new CustomerEmail { Email = item.Email });
                        }
                        ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));
                        return;
                    }
                case "Khác":
                    {
                        ListCustomerEmail = new ObservableCollection<CustomerEmail>();
                        ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));
                        return;
                    }
                case "Khách hàng mới trong tháng":
                    {
                        ListCustomerEmail = new ObservableCollection<CustomerEmail>();
                        try
                        {
                            List<CustomerDTO> newcuslist = new List<CustomerDTO>(await CustomerService.Ins.GetNewCustomer());
                            if (newcuslist != null)
                            {
                                foreach (var it in newcuslist)
                                {
                                    ListCustomerEmail.Add(new CustomerEmail { Email = it.Email });
                                }
                            }
                            ReleaseVoucherList = new ObservableCollection<VoucherDTO>(GetRandomUnreleasedCode(ListCustomerEmail.Count * int.Parse(PerCus.Content.ToString())));
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            Console.WriteLine(e);
                            MessageBoxCustom mb = new MessageBoxCustom("", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                            throw;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                            throw;
                        }
                        return;
                    }
            }
        }


        public List<VoucherDTO> GetRandomUnreleasedCode(int quantity)
        {
            return StoreAllMini.Where(v => v.Status == VOUCHER_STATUS.UNRELEASED).Take(quantity).ToList();
        }

        bool IsExport = false;
        public void ExportVoucherFunc()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    app.Visible = false;
                    Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                    Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];

                    ws.Cells[1, 1] = "Tên đợt phát hành: " + SelectedItem.ReleaseName;
                    ws.Cells[2, 1] = "Ngày phát hành: " + DateTime.Today;
                    ws.Cells[3, 1] = "Hiệu lực đến: " + SelectedItem.FinishDate;
                    ws.Cells[4, 1] = "Số lượng: " + ReleaseVoucherList.Count;
                    ws.Cells[5, 1] = "Mệnh giá: " + Utils.Helper.FormatVNMoney(SelectedItem.ParValue);
                    ws.Cells[6, 1] = "Mặt hàng áp dụng: " + SelectedItem.ObjectType;
                    ws.Cells[8, 5] = "ID voucher";
                    ws.Cells[8, 6] = "Mã voucher";

                    int i2 = 9;
                    foreach (var item in ReleaseVoucherList)
                    {

                        ws.Cells[i2, 5] = item.Id;
                        ws.Cells[i2, 6] = item.Code;

                        i2++;
                    }
                    ws.SaveAs(sfd.FileName);
                    wb.Close();
                    app.Quit();

                    IsExport = true;
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                }
                else
                {
                    IsExport = false;
                }
            }
        }

        protected async Task<(bool, string)> sendHtmlEmail(List<string> customerEmailList, List<List<string>> ListCodePerEmailList)
        {
            List<Task> listSendEmailTask = new List<Task>();
            for (int i = 0; i < customerEmailList.Count; i++)
            {
                listSendEmailTask.Add(sendEmailForACustomer(customerEmailList[i], ListCodePerEmailList[i]));
            }

            try
            {
                await Task.WhenAll(listSendEmailTask);
                return (true, "Gửi thành công");
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBoxCustom mb = new MessageBoxCustom("", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                mb.ShowDialog();
                throw;
            }
        }

        private Task sendEmailForACustomer(string customerEmail, List<string> listCode)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string APP_EMAIL = appSettings["APP_EMAIL"];
            string APP_PASSWORD = appSettings["APP_PASSWORD"];

            //SMTP CONFIG
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(APP_EMAIL, APP_PASSWORD);

            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;

            //create Alrternative HTML view

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(GetCustomerGratitudeTemplate(listCode), Encoding.UTF8, "text/html");
            //Add Image
            LinkedResource image = new LinkedResource(Helper.GetImagePath("poster.png"), "image/png");
            image.ContentId = "myImageID";
            image.ContentType.Name = "thank_you_picture";
            image.TransferEncoding = TransferEncoding.Base64;
            image.ContentLink = new Uri("cid:" + image.ContentId);

            //Add the Image to the Alternate view
            htmlView.LinkedResources.Add(image);
            //Add view to the Email Message
            mail.AlternateViews.Add(htmlView);

            mail.From = new MailAddress(APP_EMAIL, "Squadin Cinema");
            mail.To.Add(customerEmail);
            mail.Subject = "Tri ân khách hàng thân thiết";

            return smtp.SendMailAsync(mail);
        }

        private string GetCustomerGratitudeTemplate(List<string> listCode)
        {
            string templateHTML = Helper.GetEmailTemplatePath(GRATITUDE_TEMPLATE_FILE);
            string listVoucherHTML = "";

            for (int i = 0; i < listCode.Count; i++)
            {
                listVoucherHTML += VOUCHER_ITEM_HTML.Replace("{INDEX}", $"{i + 1}").Replace("{CODE_HERE}", listCode[i]);
            }


            String HTML = File.ReadAllText(templateHTML).Replace("{LIST_CODE_HERE}", listVoucherHTML);
            return HTML;
        }

        public List<List<string>> ChunkBy(List<string> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        const string GRATITUDE_TEMPLATE_FILE = "top5_customer_gratitude_html.txt";
        const string VOUCHER_ITEM_HTML = "<li>Voucher {INDEX}: {CODE_HERE}</li>";

    }

    public class CustomerEmail
    {
        public string Email { get; set; }
        public CustomerEmail() { }
    }
}
