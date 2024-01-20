using CRUD_Dapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace CRUD_Dapper.Controllers
{
    public class PessoasController : Controller
    {
        private readonly string ConnectionString = "User ID=postgres;Password=0919;Host=localhost;Port=5432;Database=PessoasDB";

        public IActionResult Index()
        {
            IDbConnection con;
            try
            {
                string selecaoQuery = "SELECT * FROM pessoas";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                IEnumerable<Pessoas> listaPessoas = con.Query<Pessoas>(selecaoQuery).ToList();
                return View(listaPessoas);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [HttpGet]
        
        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Create(Pessoas pessoas)
        {
            if (ModelState.IsValid) {
                IDbConnection con;

                try
                {
                    string InsercaoQuery = "INSERT INTO pessoas(nome, idade, peso) VALUES(@Nome, @Idade, @Peso)";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(InsercaoQuery, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return View(pessoas);
        }

        [HttpGet]

        public IActionResult Edit(int pessoaid) 
        {
            IDbConnection con;
            try
            {
                string selecaoQuery = "SELECT * FROM pessoas WHERE pessoaid = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                Pessoas pessoas = con.Query<Pessoas>(selecaoQuery).FirstOrDefault();
                con.Close();
                return View(pessoas);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]

        public IActionResult Edit(int pessoaid, Pessoas pessoas)
        {

            if (pessoaid!=pessoas.PessoaID)
                return NotFound();

            if(ModelState.IsValid)
            {
                IDbConnection con;
                try
                {
                    con = new NpgsqlConnection(ConnectionString);
                    string atualizarQuery = "UPDATE pessoas SET  Nome=@Nome, Idade=@Idade, Peso=@Peso WHERE PessoaID = @pessoaid";
                    con.Open();
                    con.Execute(atualizarQuery, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return View(pessoas);
        }

        [HttpPost]

        public IActionResult Delete(int pessoaid)
        {
            IDbConnection con;
            try
            {
                string excluirQuery = "DELETE FROM pessoas WHERE PessoaID = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                con.ExecuteAsync(excluirQuery, new {pessoaid = @pessoaid});
                Console.WriteLine($"{pessoaid}");
                con.Close();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }

        }

    }
}
