using BusinessLayer;
using DataAccessLayer;
using Moq;
using POCOs;

namespace BusinessLayerTests
{
    public class MeterReadingServiceTests
    {
        private Mock<IMeterReadingRepository> _meterReadingRepository;
        private Mock<IAccountRepository> _accountRepository;
        private MeterReadingService _service;

        [SetUp]
        public void Setup()
        {
            _meterReadingRepository = new Mock<IMeterReadingRepository>();
            _accountRepository = new Mock<IAccountRepository>();
            _service = new MeterReadingService(_meterReadingRepository.Object, _accountRepository.Object);
        }

        [Test]
        public async Task IsNotNullTest()
        {
            // Arrange
            var csvData = "";
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            // Act
            var result = await _service.Upload(stream);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.SuccessfulRecords, Is.EqualTo(0));
            Assert.That(result.FailedRecords, Is.EqualTo(0));
        }

        [Test]
        public async Task HeadersOnlyTest()
        {
            // Arrange
            var csvData = "AccountId,MeterReadingDateTime,MeterReadValue,";
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            // Act
            var result = await _service.Upload(stream);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.SuccessfulRecords, Is.EqualTo(0));
            Assert.That(result.FailedRecords, Is.EqualTo(0));
        }

        [Test]
        public async Task OneValidRecord()
        {
            // Arrange
            var csvData = @"AccountId,MeterReadingDateTime,MeterReadValue,
2344,22/04/2019 09:24,1002,";
            var validAccounts = new List<Account> { new Account { AccountId = 2344, FirstName = "John", LastName = "Doe" } };
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            _accountRepository.Setup(x => x.GetAccounts()).ReturnsAsync(validAccounts);
            _meterReadingRepository.Setup(x => x.SaveReadings(It.IsAny<IEnumerable<MeterReading>>())).ReturnsAsync(1);

            // Act
            var result = await _service.Upload(stream);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.SuccessfulRecords, Is.EqualTo(1));
            Assert.That(result.FailedRecords, Is.EqualTo(0));
        }


        [Test]
        public async Task TestFile()
        {
            // Arrange
            var csvData = @"AccountId,MeterReadingDateTime,MeterReadValue,
2344,22/04/2019 09:24,1002,
2233,22/04/2019 12:25,323,
8766,22/04/2019 12:25,3440,
2344,22/04/2019 12:25,1002,
2345,22/04/2019 12:25,45522,
2346,22/04/2019 12:25,999999,
2347,22/04/2019 12:25,54,
2348,22/04/2019 12:25,123,
2349,22/04/2019 12:25,0,
2350,22/04/2019 12:25,5684,
2351,22/04/2019 12:25,57579,
2352,22/04/2019 12:25,455,
2353,22/04/2019 12:25,1212,
2354,22/04/2019 12:25,889,
2355,06/05/2019 09:24,1,
2356,07/05/2019 09:24,0,
2344,08/05/2019 09:24,0,
6776,09/05/2019 09:24,-6575,
6776,10/05/2019 09:24,23566,
4534,11/05/2019 09:24,0,
1234,12/05/2019 09:24,9787,
1235,13/05/2019 09:24,0,
1236,10/04/2019 19:34,8898,
1237,15/05/2019 09:24,3455,
1238,16/05/2019 09:24,0,
1239,17/05/2019 09:24,45345,
1240,18/05/2019 09:24,978,
1241,11/04/2019 09:24,436,X
1242,20/05/2019 09:24,124,
1243,21/05/2019 09:24,77,
1244,25/05/2019 09:24,3478,
1245,25/05/2019 14:26,676,
1246,25/05/2019 09:24,3455,
1247,25/05/2019 09:24,3,
1248,26/05/2019 09:24,3467,
";
            var validAccounts = new List<Account> {
                new Account { AccountId = 2344, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2233, FirstName = "a", LastName = "b" },
                new Account { AccountId = 8766, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2345, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2346, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2347, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2348, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2349, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2350, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2351, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2352, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2353, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2355, FirstName = "a", LastName = "b" },
                new Account { AccountId = 2356, FirstName = "a", LastName = "b" },
                new Account { AccountId = 6776, FirstName = "a", LastName = "b" },
                new Account { AccountId = 4534, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1234, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1239, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1240, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1241, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1242, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1243, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1244, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1245, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1246, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1247, FirstName = "a", LastName = "b" },
                new Account { AccountId = 1248, FirstName = "a", LastName = "b" }
            };
            using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvData));

            _accountRepository.Setup(x => x.GetAccounts()).ReturnsAsync(validAccounts);
            _meterReadingRepository.Setup(x => x.SaveReadings(It.IsAny<IEnumerable<MeterReading>>())).ReturnsAsync(28);

            // Act
            var result = await _service.Upload(stream);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.SuccessfulRecords, Is.EqualTo(28));
            Assert.That(result.FailedRecords, Is.EqualTo(7));
        }
    }
}