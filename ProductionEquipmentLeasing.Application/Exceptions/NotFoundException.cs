using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionEquipmentLeasing.Application.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }

    // Optional: For more detailed error handling
    public string EntityName { get; }
    public object Key { get; }

    public NotFoundException(string entityName, object key, Exception? innerException = null)
        : base($"Entity \"{entityName}\" ({key}) was not found.", innerException)
    {
        EntityName = entityName;
        Key = key;
    }
}
public class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }
}