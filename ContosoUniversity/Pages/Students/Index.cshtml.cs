using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public IndexModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        public PaginatedList<Student> Student { get;set; }

        // свойства для хранения параметров сортировки:
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        // принимает параметр sortOrder из строки запроса в URL-адресе.
        // URL-адрес (включая строку запроса) формируется вспомогательной функцией тегов привязки Параметр sortOrder имеет значение "Name" или "Date".
        // После параметра sortOrder может стоять "_desc", чтобы указать порядок по убыванию.
        // По умолчанию используется порядок сортировки по возрастанию.
        // При запросе страницы "Index" по ссылке Students строка запроса отсутствует.
        // Учащиеся отображаются по фамилии в порядке возрастания.
        // В операторе switch сортировка по фамилии в порядке возрастания используется по умолчанию.
        // Когда пользователь щелкает ссылку заголовка столбца, в строке запроса указывается соответствующее значение sortOrder.
        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            // Для формирования гиперссылок в заголовках столбцов страница Razor использует NameSort и DateSort с соответствующими значениями строки запроса:
            // sortOrder равен null или пуст, NameSort имеет значение "name_desc". Если sortOrder не является равным null или пустым, для NameSort задается пустая строка.
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            CurrentFilter = searchString;
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            // инициализирует IQueryable<Student> до оператора switch и изменяет его.
            // При создании или изменении IQueryable запрос в базу данных не отправляется.
            // Запрос не выполнится, пока объект IQueryable не будет преобразован в коллекцию.
            // Таким образом, код IQueryable создает одиночный запрос, который не выполняется ToListAsync();
            IQueryable<Student> studentIQ = from s in _context.Student
                select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                studentIQ = studentIQ.Where(s => s.LastName.Contains(searchString)
                                                 || s.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentIQ = studentIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentIQ = studentIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentIQ = studentIQ.OrderBy(s => s.LastName);
                    break;
            }

            // IQueryable преобразуются в коллекцию путем вызова метода, такого как ToListAsync. 
            int pageSize = 10;
            Student = await PaginatedList<Student>.CreateAsync(
                studentIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
