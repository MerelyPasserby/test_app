using Microsoft.EntityFrameworkCore;
using test_app.Models;

namespace test_app.Services
{
    public class DBService
    {
        private readonly AutoWorkshopContext _context;

        public DBService(AutoWorkshopContext context)
        {
            _context = context;
        }

        // CARS

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {            
            return await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async System.Threading.Tasks.Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CarExistsAsync(int id)
        {
            return await _context.Cars.AnyAsync(c => c.Id == id);
        }

        // CLIENTS

        public async Task<List<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async System.Threading.Tasks.Task AddClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateClientAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ClientExistsAsync(int id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }



        // CLIENT CARS

        public async Task<List<ClientCar>> GetAllClientCarsAsync()
        {
            return await _context.ClientCars
                .Include(cc => cc.Client)
                .Include(cc => cc.Car)
                .ToListAsync();
        }

        public async Task<ClientCar?> GetClientCarByIdAsync(int id)
        {
            return await _context.ClientCars
                .Include(cc => cc.Client)
                .Include(cc => cc.Car)
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }

        public async System.Threading.Tasks.Task AddClientCarAsync(ClientCar clientCar)
        {
            _context.ClientCars.Add(clientCar);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateClientCarAsync(ClientCar clientCar)
        {
            _context.ClientCars.Update(clientCar);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteClientCarAsync(int id)
        {
            var clientCar = await _context.ClientCars.FindAsync(id);
            if (clientCar != null)
            {
                _context.ClientCars.Remove(clientCar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ClientCarExistsAsync(int id)
        {
            return await _context.ClientCars.AnyAsync(cc => cc.Id == id);
        }

        // PARTS

        public async Task<List<Part>> GetAllPartsAsync()
        {
            return await _context.Parts.ToListAsync();
        }

        public async Task<Part?> GetPartByIdAsync(int id)
        {
            return await _context.Parts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async System.Threading.Tasks.Task AddPartAsync(Part part)
        {
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdatePartAsync(Part part)
        {
            _context.Parts.Update(part);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeletePartAsync(int id)
        {
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PartExistsAsync(int id)
        {
            return await _context.Parts.AnyAsync(p => p.Id == id);
        }

        // SUPPLIERS

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async System.Threading.Tasks.Task AddSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SupplierExistsAsync(int id)
        {
            return await _context.Suppliers.AnyAsync(s => s.Id == id);
        }

        // TASK TYPES

        public async Task<List<TaskType>> GetAllTaskTypesAsync()
        {
            return await _context.TaskTypes.ToListAsync();
        }

        public async Task<TaskType?> GetTaskTypeByIdAsync(int id)
        {
            return await _context.TaskTypes.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task AddTaskTypeAsync(TaskType taskType)
        {
            _context.TaskTypes.Add(taskType);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskTypeAsync(TaskType taskType)
        {
            _context.TaskTypes.Update(taskType);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskTypeAsync(int id)
        {
            var taskType = await _context.TaskTypes.FindAsync(id);
            if (taskType != null)
            {
                _context.TaskTypes.Remove(taskType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TaskTypeExistsAsync(int id)
        {
            return await _context.TaskTypes.AnyAsync(t => t.Id == id);
        }

        // PART COMPATIBILITY

        public async Task<List<PartCompatibility>> GetAllPartCompatibilitiesAsync()
        {
            return await _context.PartCompatibilities
                .Include(p => p.Car)
                .Include(p => p.Part)
                .ToListAsync();
        }

        public async Task<PartCompatibility?> GetPartCompatibilityByIdAsync(int id)
        {
            return await _context.PartCompatibilities
                .Include(p => p.Car)
                .Include(p => p.Part)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async System.Threading.Tasks.Task AddPartCompatibilityAsync(PartCompatibility partCompatibility)
        {
            _context.PartCompatibilities.Add(partCompatibility);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdatePartCompatibilityAsync(PartCompatibility partCompatibility)
        {
            _context.PartCompatibilities.Update(partCompatibility);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeletePartCompatibilityAsync(int id)
        {
            var pc = await _context.PartCompatibilities.FindAsync(id);
            if (pc != null)
            {
                _context.PartCompatibilities.Remove(pc);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PartCompatibilityExistsAsync(int id)
        {
            return await _context.PartCompatibilities.AnyAsync(p => p.Id == id);
        }

        // PART SUPPLIER

        public async Task<List<PartSupplier>> GetAllPartSuppliersAsync()
        {
            return await _context.PartSuppliers
                .Include(p => p.Part)
                .Include(p => p.Supplier)
                .ToListAsync();
        }

        public async Task<PartSupplier?> GetPartSupplierByIdAsync(int id)
        {
            return await _context.PartSuppliers
                .Include(p => p.Part)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async System.Threading.Tasks.Task AddPartSupplierAsync(PartSupplier partSupplier)
        {
            _context.PartSuppliers.Add(partSupplier);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdatePartSupplierAsync(PartSupplier partSupplier)
        {
            _context.PartSuppliers.Update(partSupplier);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeletePartSupplierAsync(int id)
        {
            var partSupplier = await _context.PartSuppliers.FindAsync(id);
            if (partSupplier != null)
            {
                _context.PartSuppliers.Remove(partSupplier);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PartSupplierExistsAsync(int id)
        {
            return await _context.PartSuppliers.AnyAsync(p => p.Id == id);
        }

        // INSPECTIONS

        public async Task<List<Inspection>> GetAllInspectionsAsync()
        {
            return await _context.Inspections
                .Include(i => i.ClientCar)
                    .ThenInclude(cc => cc.Client)
                .Include(i => i.ClientCar)
                    .ThenInclude(cc => cc.Car)
                .ToListAsync();
        }

        public async Task<Inspection?> GetInspectionByIdAsync(int id)
        {
            return await _context.Inspections
                .Include(i => i.ClientCar)
                    .ThenInclude(cc => cc.Client)
                .Include(i => i.ClientCar)
                    .ThenInclude(cc => cc.Car)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async System.Threading.Tasks.Task AddInspectionAsync(Inspection inspection)
        {
            var tt = await _context.TaskTypes.FirstOrDefaultAsync(tt => tt.Name == "Огляд");
            inspection.Cost = tt?.LaborCost ?? 0;

            _context.Inspections.Add(inspection);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateInspectionAsync(Inspection inspection)
        {
            _context.Inspections.Update(inspection);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteInspectionAsync(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection != null)
            {
                _context.Inspections.Remove(inspection);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> InspectionExistsAsync(int id)
        {
            return await _context.Inspections.AnyAsync(i => i.Id == id);
        }

        // TASKS

        public async Task<List<Models.Task>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.Inspection)
                    .ThenInclude(i => i.ClientCar)
                        .ThenInclude(cc => cc.Car)
                .Include(t => t.Inspection)
                    .ThenInclude(i => i.ClientCar)
                        .ThenInclude(cc => cc.Client)
                .Include(t => t.TaskType)
                .ToListAsync();
        }

        public async Task<Models.Task?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Inspection)
                    .ThenInclude(i => i.ClientCar)
                        .ThenInclude(cc => cc.Car)
                .Include(t => t.Inspection)
                    .ThenInclude(i => i.ClientCar)
                        .ThenInclude(cc => cc.Client)
                .Include(t => t.TaskType)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task AddTaskAsync(Models.Task task)
        {
            var type = await _context.TaskTypes.FirstOrDefaultAsync(t => t.Id == task.TaskTypeId);
            if (type != null)
            {
                task.TotalCost = type.LaborCost;
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {                
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                    return;

                var partsToTasks = await _context.PartsToTasks
                    .Where(p => p.TaskId == id)
                    .ToListAsync();
                
                _context.PartsToTasks.RemoveRange(partsToTasks);
                
                _context.Tasks.Remove(task);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        //{
        //    var task = await _context.Tasks.FindAsync(id);
        //    if (task != null)
        //    {
        //        _context.Tasks.Remove(task);
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<bool> TaskExistsAsync(int id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }

        // PART TO TASK

        public async System.Threading.Tasks.Task AddPartToTaskAsync(PartsToTask partsToTask)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var part = await _context.Parts.FindAsync(partsToTask.PartId);
                if (part == null)
                    throw new Exception("Part not found.");

                if (partsToTask.Quantity > part.StockQuantity)
                    throw new Exception("Not enough parts in stock.");

                partsToTask.PriceAtUse = part.Price;
                part.StockQuantity -= partsToTask.Quantity;

                _context.PartsToTasks.Add(partsToTask);

                var task = await _context.Tasks.FindAsync(partsToTask.TaskId);
                if (task == null)
                    throw new Exception("Task not found.");

                task.TotalCost += part.Price * partsToTask.Quantity;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async System.Threading.Tasks.Task DeletePartFromTaskAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var item = await _context.PartsToTasks
                    .Include(p => p.Part)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (item == null)
                    throw new Exception("PartToTask not found.");

                var task = await _context.Tasks.FindAsync(item.TaskId);
                if (task == null)
                    throw new Exception("Task not found.");

                task.TotalCost -= item.PriceAtUse * item.Quantity;
                item.Part.StockQuantity += item.Quantity;

                _context.PartsToTasks.Remove(item);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }              

        public async Task<PartsToTask?> GetPartToTaskAsync(int id)
        {
            return await _context.PartsToTasks
                .Include(p => p.Part)
                .Include(p => p.Task)
                    .ThenInclude(t => t.Inspection)
                        .ThenInclude(i => i.ClientCar)
                            .ThenInclude(cc => cc.Car)
                .Include(p => p.Task)
                    .ThenInclude(t => t.Inspection)
                        .ThenInclude(i => i.ClientCar)
                            .ThenInclude(cc => cc.Client)
                .Include(p => p.Task)
                    .ThenInclude(t => t.TaskType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PartsToTask>> GetAllPartsToTasksAsync()
        {
            return await _context.PartsToTasks
                .Include(p => p.Part)
                .Include(p => p.Task)
                    .ThenInclude(t => t.Inspection)
                        .ThenInclude(i => i.ClientCar)
                            .ThenInclude(cc => cc.Car)
                .Include(p => p.Task)
                    .ThenInclude(t => t.Inspection)
                        .ThenInclude(i => i.ClientCar)
                            .ThenInclude(cc => cc.Client)
                .Include(p => p.Task)
                    .ThenInclude(t => t.TaskType)
                .ToListAsync();
        }


    }
}
