using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public CreateModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // TryUpdateModelAsync<Student> пытается обновить объект emptyStudent,
            // используя отправленные значения формы из свойства PageContext в PageModel.
            // TryUpdateModelAsync обновляет только перечисленные свойства
            // (s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate).
            // Второй аргумент("student", // Prefix) представляет собой префикс для поиска значений.
            // Задается без учета регистра символов.
            // Отправленные значения формы преобразуются в типы в модели Student с использованием привязки модели https://docs.microsoft.com/ru-ru/aspnet/core/mvc/models/model-binding?view=aspnetcore-2.2#how-model-binding-works .
            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student",   // Prefix for form value.
                s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate))
            {
                _context.Student.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            return null;
        }
    }
}