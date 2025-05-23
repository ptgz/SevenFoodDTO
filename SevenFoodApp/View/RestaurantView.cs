using SevenFoodApp.Controller;
using SevenFoodApp.Interfaces;
using SevenFoodApp.Model;
using SevenFoodApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SevenFoodApp.Util.Enums;

namespace SevenFoodApp.View
{
    internal class RestaurantView : IView
    {
        RestaurantController controller = new RestaurantController();
        public RestaurantRequestDTO restaurantrDTO = new RestaurantRequestDTO();
        public void Add()
        {
            try
            {
                Console.WriteLine("CADASTRAR NOVO RESTAURANTE\n");
                Console.Write("Nome: ");
                string name = Please.ConsoleRead() ?? "Nome não Informado";
                restaurantrDTO.Name = name;

                if (this.controller.Add(restaurantrDTO))
                    Console.WriteLine("Restaurante Cadastrado com sucesso.");
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
                Console.WriteLine("Restaurante removido com sucesso.");
            else
                Console.WriteLine(Please.GetMessageGenericError());
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
                Console.WriteLine($"Restaurante não existe para o id {id}");
            }
        }
        public void Show(Dictionary<string, string> obj)
        {
            if (obj != null)
            {
                Console.WriteLine("CADASTRO DO RESTAURANTE\n");
                Console.WriteLine($"Id    : {obj["id"]}");
                Console.WriteLine($"Nome  : {obj["name"]}");
                Console.WriteLine($"Senha : {obj["password"]}");
                Console.WriteLine($"Ativo : {obj["active"]}");
            }

        }

        public void ShowAll()
        {
            this.ShowTitle();
            var objs = controller.getAll();

            if (objs != null && objs.Count() > 0)
            {
                foreach (RestaurantResponseDTO obj in objs)
                {
                    this.ShowInLine(obj);
                }
            }
        }

        public void Update()
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

                Console.WriteLine("ATUALIZE OS DADOS DO USUÁRIO");
                Console.Write("Nome: ");
                string name = Please.ConsoleRead() ?? "";
                name = name == "" ? obj["name"] : name;

                Console.Write("Status: (1 - Ativo | 0 - Inativo)");
                string active = Please.ConsoleRead()!;
                bool status;
                switch (active)
                {
                    case "":
                        status = Please.TranslateToBool(obj["active"]); break;
                    case "1":
                        status = true; break;
                    default:
                        status = false; break;
                }

                if (controller.update(id, name, status))
                    Console.WriteLine("Restaurante Atualizado com sucesso.");
                else
                    Console.WriteLine(Please.GetMessageGenericError());
            }
            else
            {
                Console.WriteLine($"Restaurante não existe para o id {id}");
            }
        }

        private void ShowInLine(RestaurantResponseDTO obj)
        {
            Console.Write($"{obj.Id.ToString().PadRight((int)SIZE.FIVE)}");
            Console.Write($"{obj.Name.ToString().PadRight((int)SIZE.THIRTY)[..((int)SIZE.THIRTY - 1)]} ");
            Console.Write($"{obj.Active.ToString().PadRight((int)SIZE.FIFTEEN)}");
            Console.WriteLine("");
        }

        private void ShowTitle()
        {
            int totalSize = (int)SIZE.FIVE + (int)SIZE.TEN + (int)SIZE.THIRTY;
            Console.WriteLine($"".PadRight(totalSize, '-'));
            Console.Write($"ID".PadRight((int)SIZE.FIVE));
            Console.Write($"NOME".PadRight((int)SIZE.THIRTY));
            Console.WriteLine($"ATIVO".PadRight((int)SIZE.TEN));
            Console.WriteLine($"".PadRight(totalSize, '-'));
        }
    }
}
