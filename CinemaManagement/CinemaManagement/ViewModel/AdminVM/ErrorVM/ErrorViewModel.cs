using CinemaManagement.DTOs;
using CinemaManagement.Models.Services;
using CinemaManagement.Views;
using CinemaManagement.Views.Admin.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CinemaManagement.ViewModel
{
    public partial class MainAdminViewModel : BaseViewModel
    {
        private ComboBoxItem _SelectedFilterList;
        public ComboBoxItem SelectedFilterList
        {
            get { return _SelectedFilterList; }
            set { _SelectedFilterList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TroubleDTO> listError;
        public ObservableCollection<TroubleDTO> ListError
        {
            get { return listError; }
            set { listError = value; OnPropertyChanged(); }
        }

        private TroubleDTO _SelectedItem;
        public TroubleDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private ComboBoxItem selectedStatus;
        public ComboBoxItem SelectedStatus
        {
            get { return selectedStatus; }
            set { selectedStatus = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { _SelectedDate = value; OnPropertyChanged(); }
        }

        private DateTime _SelectedFinishDate;
        public DateTime SelectedFinishDate
        {
            get { return _SelectedFinishDate; }
            set { _SelectedFinishDate = value; OnPropertyChanged(); }
        }

        private decimal _RepairCost;
        public decimal RepairCost
        {
            get { return _RepairCost; }
            set { _RepairCost = value; OnPropertyChanged(); }
        }


        public ICommand LoadDetailErrorCM { get; set; }
        public ICommand UpdateErrorCM { get; set; }
        public ICommand ReloadErrorListCM { get; set; }

        public void ChoseWindow()
        {
            if (SelectedItem.Status == Utils.STATUS.DONE)
            {
                DoneError w = new DoneError();
                w.ShowDialog();
            }
            else if (SelectedItem.Status == Utils.STATUS.WAITING)
            {
                WaitingError w = new WaitingError();
                w.ShowDialog();
            }
            else if (SelectedItem.Status == Utils.STATUS.IN_PROGRESS)
            {
                InprogressError w = new InprogressError();
                w.ShowDialog();
            }
        }

        public async Task<List<TroubleDTO>> GetAllTrouble()
        {
            try
            {
                return await TroubleService.Ins.GetAllTrouble();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        public async Task ReloadErrorList()
        {
            try
            {
                if (SelectedFilterList is null) return;

                List<TroubleDTO> troubleDTOs = await GetAllTrouble();

                ListError = new ObservableCollection<TroubleDTO>();

                //reduce the number notifi of main page
                int counttemp = 0;
                foreach (var item in troubleDTOs)
                {
                    if (item.Status == Utils.STATUS.WAITING)
                        counttemp++;
                }
                ErrorCount = counttemp.ToString();
                ///================

                if ((string)SelectedFilterList.Tag == "Toàn bộ")
                {
                    ListError = new ObservableCollection<TroubleDTO>(troubleDTOs);
                }
                else
                {
                    ListError = new ObservableCollection<TroubleDTO>(troubleDTOs.Where(tr => tr.Status == SelectedFilterList.Tag.ToString()));
                }

                //if (SelectedFilterList.Content.ToString() == Utils.STATUS.WAITING)
                //{
                //    foreach (var item in temp)
                //    {
                //        if (item.Status == SelectedFilterList.Content.ToString())
                //            ListError.Add(item);
                //    }
                //    return;
                //}
                //if (SelectedFilterList.Content.ToString() == Utils.STATUS.IN_PROGRESS)
                //{
                //    foreach (var item in temp)
                //    {
                //        if (item.Status == SelectedFilterList.Content.ToString())
                //            ListError.Add(item);
                //    }
                //    return;
                //}
                //if (SelectedFilterList.Content.ToString() == Utils.STATUS.DONE)
                //{
                //    foreach (var item in temp)
                //    {
                //        if (item.Status == SelectedFilterList.Content.ToString())
                //            ListError.Add(item);
                //    }
                //    return;
                //}
                //if (SelectedFilterList.Content.ToString() == Utils.STATUS.CANCLE)
                //{
                //    foreach (var item in temp)
                //    {
                //        if (item.Status == SelectedFilterList.Content.ToString())
                //            ListError.Add(item);
                //    }
                //    return;
                //}

                //ListError = new ObservableCollection<TroubleDTO>(temp);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task UpdateErrorFunc(Window p)
        {
            if (SelectedStatus.Content.ToString() == Utils.STATUS.IN_PROGRESS)
            {
                if (DateTime.Compare(SelectedItem.SubmittedAt.Date, SelectedDate.Date) > 0)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", "Ngày không hợp lệ!", MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                    return;
                }

                TroubleDTO trouble = new TroubleDTO
                {
                    Id = SelectedItem.Id,
                    StartDate = SelectedDate,
                    Status = SelectedStatus.Content.ToString(),
                };
                (bool isS, string messageFromUpdate) = await TroubleService.Ins.UpdateStatusTrouble(trouble);

                if (isS)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdate, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdate, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }

            }
            else if (SelectedStatus.Content.ToString() == Utils.STATUS.CANCLE)
            {
                TroubleDTO trouble = new TroubleDTO
                {
                    Id = SelectedItem.Id,
                    Status = SelectedStatus.Content.ToString(),
                };

                (bool isS, string messageFromUpdate) = await TroubleService.Ins.UpdateStatusTrouble(trouble);

                if (isS)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdate, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdate, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }
            }
            else if (SelectedStatus.Content.ToString() == Utils.STATUS.DONE)
            {
                if (SelectedItem.StartDate.HasValue)
                {
                    DateTime t = SelectedItem.StartDate.Value;
                    if (DateTime.Compare(t.Date, SelectedFinishDate.Date) > 0)
                    {
                        MessageBoxCustom mb = new MessageBoxCustom("", "Ngày không hợp lệ!", MessageType.Error, MessageButtons.OK);
                        mb.ShowDialog();
                        return;
                    }
                }

                TroubleDTO trouble = new TroubleDTO
                {
                    Id = SelectedItem.Id,
                    FinishDate = SelectedFinishDate,
                    Status = SelectedStatus.Content.ToString(),
                    RepairCost = RepairCost,
                };

                (bool isS, string messageFromUpdate) = await TroubleService.Ins.UpdateStatusTrouble(trouble);

                if (isS)
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdate, MessageType.Success, MessageButtons.OK);
                    mb.ShowDialog();
                    await ReloadErrorList();
                    p.Close();
                }
                else
                {
                    MessageBoxCustom mb = new MessageBoxCustom("", messageFromUpdate, MessageType.Error, MessageButtons.OK);
                    mb.ShowDialog();
                }

            }
        }
    }

}
