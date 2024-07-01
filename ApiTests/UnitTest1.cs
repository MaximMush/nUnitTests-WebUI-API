using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ApiTests
{
    [TestFixture]
    public class Tests
    {
        private HttpClient client;
        
        private const string BaseUrl = "https://petstore.swagger.io/v2/pet";
        
        private long petId;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient();
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Test, Order(1)]
        public async Task CreatePet()
        {
            var pet = new
            {
                id = 123456,
                name = "Fluffy",
                category = new { id = 1, name = "Dogs" },
                photoUrls = new[] { "http://example.com/dog.jpg" },
                tags = new[] { new { id = 1, name = "Friendly" } },
                status = "available"
            };

            var response = await client.PostAsJsonAsync(BaseUrl, pet);
            
            response.EnsureSuccessStatusCode();

            var createdPet = await response.Content.ReadFromJsonAsync<Pet>();
            
            Assert.IsNotNull(createdPet);
            
            Assert.AreEqual("Fluffy", createdPet.name);
            
            petId = createdPet.id; 
        }
        
        [Test, Order(2)]
        public async Task GetPetById()
        {
            var response = await client.GetAsync($"{BaseUrl}/{petId}");
            
            response.EnsureSuccessStatusCode();

            var pet = await response.Content.ReadFromJsonAsync<Pet>();
            
            Assert.IsNotNull(pet);
            
            Assert.AreEqual(petId, pet.id);
        }

        [Test, Order(3)]
        public async Task UpdatePet()
        {
            var updatedPet = new
            {
                id = petId,
                name = "Fluffy Updated",
                status = "sold"
            };

            var response = await client.PutAsJsonAsync(BaseUrl, updatedPet);
            
            response.EnsureSuccessStatusCode();

            var pet = await client.GetFromJsonAsync<Pet>($"{BaseUrl}/{petId}");
            
            Assert.AreEqual("Fluffy Updated", pet.name);
            
            Assert.AreEqual("sold", pet.status);
        }

        [Test, Order(4)]
        public async Task DeletePet()
        {
            var response = await client.DeleteAsync($"{BaseUrl}/{petId}");
            
            response.EnsureSuccessStatusCode();

            var deletedPetResponse = await client.GetAsync($"{BaseUrl}/{petId}");
            
            Assert.AreEqual(System.Net.HttpStatusCode.NotFound, deletedPetResponse.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }

        public class Pet
        {
            public int id { get; set; }
            public string name { get; set; }
            public string status { get; set; }
        }
    }
}
