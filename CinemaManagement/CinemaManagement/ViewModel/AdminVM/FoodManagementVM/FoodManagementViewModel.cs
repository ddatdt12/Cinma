using CinemaManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.ViewModel.AdminVM.FoodManagementVM
{
    public partial class FoodManagementViewModel : BaseViewModel
    {

        private ObservableCollection<ProductDTO> _foodList;
        public ObservableCollection<ProductDTO> FoodList
        {
            get => _foodList;
            set
            {
                _foodList = value;
            }
        }

        public FoodManagementViewModel()
        {
            FoodList = new ObservableCollection<ProductDTO>() { };
            for (int i = 0; i < 6; i++)
            {
                FoodList.Add(new ProductDTO("Bap ngo","Do an",10000));
            }

        }
    }
    
}
