using Planify_BackEnd.Models;

public interface ICampusRepository
    {
         Task<IEnumerable<Campus>> getAllCampus();
    }

