using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab_2.Models.ViewModels
{
    public class PersonVM
    {
        public System.Guid ID { get; set; }
        [Required(ErrorMessage = "Поле Фамилия не должно быть пустым")]
        [DisplayName("Фамилия")]
        [StringLength(100, MinimumLength = 2)]
        public string last_name { get; set; }
        [Required(ErrorMessage = "Поле Имя не должно быть пустым")]
        [DisplayName("Имя")]
        public string first_name { get; set; }
        [DisplayName("Отчество")]
        public string patronymic { get; set; }
        [Required(ErrorMessage = "Поле Возраст не должно быть пустым")]
        [Range(18, 100)]
        [DisplayName("Возраст")]
        public int age { get; set; }
        [DisplayName("Пол")]
        public string gender { get; set; }
        [Required]
        [DisplayName("Трудоустроен")]
        public bool has_job { get; set; }

        [DisplayName ("Дата рождения")]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [DisplayName("Дата и время добавления")]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-ddThh:mm", ApplyFormatInEditMode = true)]
        public DateTime InsertDateTime { get; set; }
        [DisplayName("Время подъема")]
        [DisplayFormat(DataFormatString = "0:HH:mm", ApplyFormatInEditMode = true)]
        public DateTime WakeUpTime { get; set; }

    }
}