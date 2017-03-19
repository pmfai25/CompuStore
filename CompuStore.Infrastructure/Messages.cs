using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompuStore.Infrastructure
{
    public static class Messages
    {
        public static void ErrorValidation()
        {
            Messages.Error("يوجد خطاء في بعض البيانات");
        }
        public static void Error(string error)
        {
            MessageBox.Show(error, "error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public static bool Question(string quest)
        {
            return MessageBox.Show(quest, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public static bool Delete(string name)
        {
            return Question("هل تريد حذف " + name + " ?");
        }
        public static void ErrorDataNotSaved()
        {
            Error("حدث خطأ اثناء حفظ العميل في قاعدة البيانات");
        }

        public static void Notification(string v)
        {
            MessageBox.Show(v, "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
