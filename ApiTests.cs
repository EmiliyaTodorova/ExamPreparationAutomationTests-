using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace ContactBook.APITests
{
    public class ApiTests
    {
        public const string url = "https://contactbook.emiliyatodorova.repl.co/api/contacts";
        public RestClient client;
        private RestRequest request;


        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_GetAllClients_CheckFirstClient()
        {
            this.request = new RestRequest(url);
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].firstName +" "+ contacts[0].lastName, Is.EqualTo("Steve Jobs"));
        }
        [Test]
        public void Test_SearchClients_CheckFirstResult()
        {
            this.request = new RestRequest(url + "/search/albert");
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));
        }
        [Test]
        public void Test_SearchInvalidClients_CheckResultIsEmpty()
        {
            this.request = new RestRequest(url + "/search/invalid2635");
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));
        }
        [Test]
        public void Test_CreateInvalidClients_CheckErrorIsShow()
        {
            this.request = new RestRequest(url);
            var body = new
            {
                firstName = "Ema",
                email = "ema@abv.bg",
                phone = "12345678"
            };
            request.AddJsonBody(body);
            var response = this.client.Execute(request, Method.Post);
           
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));


        }
        [Test]
        public void Test_CreateValidClients_CheckContactIsAdded()
        {
            this.request = new RestRequest(url);
            var body = new
            {
                firstName = "Ema",
                lastName = "Todorov",
                email = "ema@abv.bg",
                phone = "12345678"
            };
            request.AddJsonBody(body);
            var response = this.client.Execute(request, Method.Post);
            
            var allContacts = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(allContacts.Content);
            var lastContact = contacts[contacts.Count - 1];

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(lastContact.firstName, Is.EqualTo("Ema"));
            Assert.That(lastContact.lastName, Is.EqualTo("Todorov"));




        }
    }
}