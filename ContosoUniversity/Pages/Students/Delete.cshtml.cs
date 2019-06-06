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
    public class DeleteModel : PageModel
    {
        private readonly ContosoUniversity.Models.SchoolContext _context;

        public DeleteModel(ContosoUniversity.Models.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; }
        public string ErrorMessage { get; set; }


        // необязательный параметр saveChangesError указывает, был ли метод вызван после того, как произошел сбой при удалении объекта учащегося.
        // Операция удаления может завершиться сбоем из-за временных проблем с сетью.
        // Вероятность возникновения временных проблем с сетью выше в облаке.
        // saveChangesError имеет значение false при вызове OnGetAsync страницы Delete из пользовательского интерфейса.
        // Если OnGetAsync вызывается методом OnPostAsync (из-за сбоя операции удаления), параметру saveChangesError присваивается значение true.
        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ErrorMessage = "Delete failed. Try again";
            }

            return Page();
        }

        // Bpdktrftn выбранную сущность и вызывает метод Remove, чтобы присвоить ей состояние Deleted.
        // При вызове метода SaveChanges создается инструкция SQL DELETE.
        // В случае сбоя Remove:Вызывается исключение базы данных.
        // Вызывается метод OnGetAsync страницы Delete с параметром saveChangesError= true.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            try
            {
                _context.Student.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("./Delete",
                    new { id, saveChangesError = true });
            }
        }
    }
}
