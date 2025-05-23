using SevenFoodApp.Controller;
using SevenFoodApp.Interfaces;
using SevenFoodApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SevenFoodApp.Util.Enums;

namespace SevenFoodApp.View
{
    internal class FoodView : IView
    {
        FoodController controller = new FoodController();
        public FoodRequestDTO frDTO = new FoodRequestDTO();
        public void Add()
        {
            try
            {
                Console.WriteLine("CADASTRAR NOVA COMIDA\n");
                Console.Write("Descrição: ");
                string description = Please.ConsoleRead() ?? "Nome não Informado";
                frDTO.Description = description;

                Console.Write("Preço: ");
                double price = double.Parse(Please.ConsoleRead() ?? "0,0");
                frDTO.Price = price;

                Console.Write("Restaurante: ");
                int idRestaurant = int.Parse(Please.ConsoleRead() ?? "0");
                frDTO.Id = idRestaurant;


                if (this.controller.Add(frDTO))
                    Console.WriteLine("Comida Cadastrada com sucesso.");
                else
                    Console.WriteLine(Please.GetMessageGenericError());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Remove()
        {
            Console.WriteLine("PESQUISAR PELO ID");
            Console.Write("Nº ID: ");
            string? idString = Please.ConsoleRead();
            int id = 0;

            if (idString != null)
            {
                int v = int.Parse(idString);
                id = v;
            }

            if (controller.remove(id))
                Console.WriteLine("Comida removida com sucesso.");
            else
                Console.WriteLine(Please.GetMessageGenericError());
        }

        public void ShowAll()
        {
            this.ShowTitle();
            var objs = controller.getAll();

            if (objs != null && objs.Count() > 0)
            {
                foreach (FoodResponseDTO obj in objs)
                {
                    this.ShowInLine(obj);
                }
            }
        }

        public void ShowById()
        {
            Console.WriteLine("PESQUISAR PELO ID");
            Console.Write("Nº ID: ");
            string? idString = Please.ConsoleRead();
            int id = 0;

            if (idString != null)
            {
                int v = int.Parse(idString);
                id = v;
            }

            var obj = controller.getById(id);

            if (obj != null)
            {
                this.Show(obj);
            }
            else
            {
                Console.WriteLine($"Comida não existe para o id {id}");
            }
        }

        public void Show(Dictionary<string, string> obj)
        {
            if (obj != null)
            {
                Console.WriteLine("CADASTRO DA COMIDA\n");
                Console.WriteLine($"Restaurante : {obj["restaurant"]}");
                Console.WriteLine($"Id          : {obj["id"]}");
                Console.WriteLine($"Descrição   : {obj["description"]}");
                Console.WriteLine($"Preço       : {obj["price"]}");
                Console.WriteLine($"Ativo       : {obj["status"]}");
            }

        }

        private void ShowInLine(FoodResponseDTO obj)
        {
            Console.Write($"{obj.Id.ToString().PadRight((int)SIZE.FIVE)}");
            Console.Write($"{obj.Description.PadRight((int)SIZE.THIRTY)[..((int)SIZE.THIRTY - 1)]} ");
            Console.Write($"{obj.Price.ToString().PadRight((int)SIZE.FIFTEEN)}");
            Console.Write($"{obj.Status.ToString().PadRight((int)SIZE.TEN)}");
            Console.Write($"{obj.Restaurant.ToString().PadRight((int)SIZE.TWENTY)}");
            Console.WriteLine("");
        }

        private void ShowTitle()
        {
            int totalSize = (int)SIZE.FIVE + (int)SIZE.TEN + (int)SIZE.FIFTEEN + (int)SIZE.THIRTY + (int)SIZE.TWENTY;
            Console.WriteLine($"".PadRight(totalSize, '-'));
            Console.Write($"ID".PadRight((int)SIZE.FIVE));
            Console.Write($"DESCRIPTION".PadRight((int)SIZE.THIRTY));
            Console.Write($"PREÇO".PadRight((int)SIZE.FIFTEEN));
            Console.Write($"ATIVO".PadRight((int)SIZE.TEN));
            Console.Write($"RESTAURANTE".PadRight((int)SIZE.TWENTY));
            Console.WriteLine($"\n--".PadRight(totalSize, '-'));
        }

        public void Update()
        {
            try
            {
                Console.WriteLine("PESQUISAR PELO ID");
                Console.Write("Nº ID: ");
                string? idString = Please.ConsoleRead();
                int id = 0;

                if (idString != null)
                {
                    int v = int.Parse(idString);
                    id = v;
                }

                Dictionary<string, string>? obj = controller.getById(id);

                if (obj != null)
                {
                    this.Show(obj);

                    Console.WriteLine("ATUALIZE OS DADOS DA COMIDA");
                    Console.Write("Descrição: ");
                    string description = Please.ConsoleRead() ?? "";
                    description = description == "" ? obj["description"] : description;

                    Console.Write("Preço: (Formato 0,00)");
                    string priceString = Please.ConsoleRead() ?? "";
                    priceString = priceString == "" ? obj["price"] : priceString;
                    double price = default;

                    try
                    {
                        price = double.Parse(priceString);
                    }
                    catch
                    {
                        throw new Exception("Preço no formato errado, Tente novamente");
                    }

                    int idRestaurant = int.Parse(obj["idRestaurant"]);

                    Console.Write("Disponível: (1 - Sim | 0 - Não)");
                    string active = Please.ConsoleRead()!;
                    bool status;
                    switch (active)
                    {
                        case "":
                            status = Please.TranslateToBool(obj["status"]); break;
                        case "1":
                            status = true; break;
                        default:
                            status = false; break;
                    }

                    if (controller.update(id, description, idRestaurant, price, status))
                        Console.WriteLine("Comida Atualizado com sucesso.");
                    else
                        Console.WriteLine(Please.GetMessageGenericError());
                }
                else
                {
                    Console.WriteLine($"Comida não existe para o id {id}");
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString()); 
            }
        }
    }
}
