using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymManagement.Domain.Rooms;

namespace GymManagement.Domain.Gyms
{
    public class Gym
    {
        private readonly int _maxRooms;

        public Guid Id { get; }
        private readonly List<Guid> _roomIds = [];
        private readonly List<Guid> _trainerIds = [];

        public string Name { get; init; } = null!;
        public Guid SubscriptionId { get; init; }

        public Gym(
            string name,
            int maxRooms,
            Guid subscriptionId,
            Guid? id = null)
        {
            Name = name;
            _maxRooms = maxRooms;
            SubscriptionId = subscriptionId;
            Id = id ?? Guid.NewGuid();
        }

        public void AddRoom(Room room)
        {
            if (_roomIds.Contains(room.Id))
            {
                throw new InvalidOperationException("Room already exists in the gym.");
            }

            if (_roomIds.Count >= _maxRooms)
            {
                // return GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows;
            }

            _roomIds.Add(room.Id);


        }

        public bool HasRoom(Guid roomId)
        {
            return _roomIds.Contains(roomId);
        }

        public void AddTrainer(Guid trainerId)
        {
            if (_trainerIds.Contains(trainerId))
            {
                //return Conflict(description: "Trainer already added to gym");
            }

            _trainerIds.Add(trainerId);

        }

        public void RemoveRoom(Guid roomId)
        {
            _roomIds.Remove(roomId);
        }

        private Gym() { }

    }
}