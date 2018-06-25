using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using login.Data;
using login.Models;
using Microsoft.EntityFrameworkCore;

namespace login.Services
{
   

    public class SqlEmpleadosData : IEmpleadosData
    {
        public ApplicationDbContext _context;
        public SqlEmpleadosData(ApplicationDbContext context)
        {
            _context = context;
        }

        public PeopleData Add(PeopleData empleado)
        {
            _context.PeopleData.Add(empleado);
            _context.SaveChanges();
            return empleado;
        }

        public GaleryData AddPicture(GaleryData picture)
        {
            _context.GaleryData.Add(picture);
            _context.SaveChanges();
            return  picture ;
        }

        public ChooseUs AddReason(ChooseUs chooseUs)
        {
            _context.ChooseUs.Add(chooseUs);
            _context.SaveChanges();
            return chooseUs;


        }

        public PeopleData Get(int id)
        {
            return _context.PeopleData.FirstOrDefault(r => r.ID == id);
        }

        public IEnumerable<PeopleData> GetAll()
        {
           return _context.PeopleData.OrderBy(r => r.Nombre);
        }

        public IEnumerable<GaleryData> GetAllPictures()
        {
            return _context.GaleryData.OrderBy(g => g.Nombre);
        }

        public IEnumerable<ServicesData> GetAllServices()
        {
            return _context.ServicesData.OrderBy(s => s.Nombre);
        }

        public IEnumerable<ChooseUs> GetAllTheReasons()
        {
            return _context.ChooseUs.OrderBy(m => m.ID);
        }

        //public GaleryData UpdateJustInfo(GaleryData updatingInfo)
        //{
        //    var nombre = updatingInfo.Nombre;


        //    _context.Entry(nombre).State = EntityState.Modified;
        //    _context.Attach(updatingInfo.Description).State = EntityState.Modified;
        //    _context.Attach(updatingInfo.Image).State = EntityState.Unchanged;

        //    _context.SaveChanges();
        //    return updatingInfo;
        //}

        //public PeopleData Update(PeopleData empleado)
        //{
        //    _context.Attach(empleado).State =
        //       EntityState.Modified;
        //    _context.SaveChanges();
        //}
    }
}
