using login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace login.Services
{
  public interface IEmpleadosData
    {
        IEnumerable<PeopleData> GetAll();
        PeopleData Get(int id);
        //PeopleData Update(PeopleData empleado);
        PeopleData Add(PeopleData newEmpleado);
        IEnumerable<ServicesData> GetAllServices();
        IEnumerable<GaleryData> GetAllPictures();
        IEnumerable<ChooseUs> GetAllTheReasons();
        IEnumerable<Notifications> GetNotifications();
        IEnumerable<PeopleData> GetEmpleados();
        GaleryData AddPicture(GaleryData newPicture);
        ChooseUs AddReason(ChooseUs chooseUs);
        Notifications AddNotification(Notifications newNotification);
     
    }
}
 