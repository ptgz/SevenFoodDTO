using SevenFoodApp.Model;
using SevenFoodApp.Repository;
using SevenFoodApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SevenFoodApp.Util.Enums;

namespace SevenFoodApp.Controller
{
    internal class FoodController
    {
        FoodRepository repository = new FoodRepository(CONTEXT.FOOD);
        RestaurantRepository restaurantRepository = new RestaurantRepository(CONTEXT.RESTAURANT);

        public bool Add(FoodRequestDTO fDTO)
        {
            try
            {
                int id = this.GetNextId();
                Restaurant? restaurant = this.restaurantRepository.GetById(fDTO.Id);
                if (restaurant == null)
                {
                    throw new Exception("Restaurant Inexistente");
                }
                Food food = new Food(id, fDTO.Description, fDTO.Price, restaurant);
                return repository.Insert(food);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Please.GetMessageGenericError());
                return false;
            }
        }

        private int GetNextId()
        {
            return Please.GetNextId();
        }

        public List<FoodResponseDTO>? getAll()
        {

            List<Food> foods = repository.GetAll();


            if (foods != null && foods.Count() > 0)
            {
                var foodsDTO = new List<FoodResponseDTO>();

                foreach (var food in foods)
                {
                    var foodDTO = this.castToDTO(food);
                    foodsDTO.Add(foodDTO);
                }
                return foodsDTO;
            }
            return null;
        }

        private FoodResponseDTO castToDTO(Food food)
        {
            FoodResponseDTO foodDTO = new FoodResponseDTO();
            foodDTO.Id = food.Id;
            foodDTO.Description = food.Description;
            foodDTO.Price = food.Price;
            foodDTO.Restaurant = food.Restaurant;

            return foodDTO;
        }

        public Dictionary<string, string>? getById(int id)
        {
            var food = repository.GetById(id);
            return food != null ? this.castObjectToDictionary(food) : null;
        }

        public bool remove(int id)
        {
            return repository.Delete(id);
        }

        internal bool update(int id, string description, int idRestaurant, double price, bool status)
        {
            try
            {
                Restaurant? restaurant = this.restaurantRepository.GetById(idRestaurant);
                if (restaurant == null)
                {
                    throw new Exception("Restaurant Inexistente");
                }

                Food Food = new Food(id, description, price, restaurant, status);
                return repository.Update(Food);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Please.GetMessageGenericError());
                return false;
            }
        }

        private Dictionary<string, string> castObjectToDictionary(Food food)
        {
            var foodString = new Dictionary<string, string>
                {
                    { "id", food.Id.ToString() },
                    { "description",  food.Description },
                    { "price",  food.Price.ToString("C2") },
                    { "restaurant",  $"{food.Restaurant.Id} - {food.Restaurant.Name}" },
                    { "idRestaurant",  food.Restaurant.Id.ToString() },
                    { "status", Please.TranslateFromBool(food.Status) },
                };
            return foodString;
        }
    }
}
