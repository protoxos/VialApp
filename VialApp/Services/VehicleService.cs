using Dapper;
using VialApp.Models;
using VialApp.Tools;

namespace VialApp.Services
{
    public class VehicleService
    {
        readonly string SQL_GET_VEHICLES_BY_OWNER = @"
                SELECT
		            v.Id,
		            v.InternalId,
		            v.Deleted,
		            v.UserCreatorId,
		            v.Alias,
		            v.Brand,
		            v.Model,
		            v.[Year],
		            v.Vin,
		            v.Color,
		            v.CreationTime
	
	            FROM 
		            Vehicle v
		            JOIN VehicleOwner vo ON vo.VehicleId = v.Id
	            WHERE 
		            vo.LastOwner = 1
		            AND vo.UserId = @UserCreatorId
                    AND v.Deleted = 0";
        readonly string SQL_GET_VEHICLE_BY_OWNER_AND_ID = @"
                SELECT
		            v.Id,
		            v.InternalId,
		            v.Deleted,
		            v.UserCreatorId,
		            v.Alias,
		            v.Brand,
		            v.Model,
		            v.[Year],
		            v.Vin,
		            v.Color,
		            v.CreationTime
	
	            FROM 
		            Vehicle v
		            JOIN VehicleOwner vo ON vo.VehicleId = v.Id
	            WHERE 
		            vo.LastOwner = 1
		            AND vo.UserId = @UserCreatorId
		            AND v.Id = @VehicleId
                    AND v.Deleted = 0";

        readonly string SQL_CREATE_VEHICLE = @"
            INSERT INTO Vehicle(InternalId, Deleted, UserCreatorId, Alias, Brand, Model, [Year], Vin, Color, CreationTime)
	        SELECT NEWID(), 0, @UserCreatorId, @Alias, @Brand, @Model, @Year, @Vin, @Color, GETDATE()
            SELECT SCOPE_IDENTITY()
        ";
        readonly string SQL_UPDATE_VEHICLE = @"
            UPDATE Vehicle
	        SET
		        Alias = @Alias,
		        Brand = @Brand,
		        Model = @Model,
		        [Year] = @Year,
		        Vin = @Vin,
		        Color = @Color
	        WHERE
		        Id = @Id
                AND Deleted = 0
        ";
        readonly string SQL_DELETE_VEHICLE = @"
            UPDATE Vehicle
	        SET
		        Deleted = @Deleted
	        WHERE
		        Id = @VehicleId
        ";
        public VehicleModel? Get(int UserCreatorId, int VehicleId)
        {
            return getVehicle(UserCreatorId, VehicleId);
        }

        public List<VehicleModel> GetAll(int UserCreatorId)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                // Obtenemos el vehiculo por usuario y Id
                var v = cnn.Query<VehicleModel>(
                    SQL_GET_VEHICLES_BY_OWNER,
                    new { UserCreatorId }
                ).ToList();

                return v;
            }
        }
        public VehicleModel? Create(int UserCreatorId, VehicleModel vehicle)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                VehicleModel? v = cnn.Query<VehicleModel>(
                    SQL_CREATE_VEHICLE,
                    new
                    {
                        UserCreatorId,
                        vehicle.Alias,
                        vehicle.Brand,
                        vehicle.Model,
                        vehicle.Year,
                        vehicle.Vin,
                        vehicle.Color
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                ).FirstOrDefault();

                return v;
            }
        }

        /// <summary>
        /// Actualiza un vehiculo y retorna los datos antes del cambio
        /// </summary>
        /// <param name="UserCreatorId">Id del usuario actual</param>
        /// <param name="Vehicle">Información del vehiculo</param>
        /// <returns></returns>
        public VehicleModel? Update(int UserCreatorId, VehicleModel Vehicle)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                // Obtenemos el vehiculo por usuario y Id
                VehicleModel? v = getVehicle(UserCreatorId, Vehicle.Id);

                // Si existe... (osease, si el vehiculo es del usuario)
                if (v != null)
                {
                    //  Updateamos
                    cnn.Execute(
                        SQL_UPDATE_VEHICLE,
                        new
                        {
                            Vehicle.Id,
                            Vehicle.Alias,
                            Vehicle.Brand,
                            Vehicle.Model,
                            Vehicle.Year,
                            Vehicle.Vin,
                            Vehicle.Color
                        }
                    );
                }

                return v;
            }
        }
        public bool Delete(int UserCreatorId, int VehicleId)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                if (getVehicle(UserCreatorId, VehicleId) != null)
                {
                    cnn.Execute(SQL_DELETE_VEHICLE, new { UserCreatorId });
                    return true;
                }
                return false;
            }
        }

        VehicleModel? getVehicle(int UserCreatorId, int VehicleId)
        {
            using (var cnn = ConnectionFactory.CreateConnection())
            {
                // Obtenemos el vehiculo por usuario y Id
                VehicleModel? v = cnn.Query<VehicleModel>(
                    SQL_GET_VEHICLE_BY_OWNER_AND_ID,
                    new
                    {
                        UserCreatorId,
                        VehicleId
                    }
                ).FirstOrDefault();

                return v;
            }
        }
    }
}
