using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using Ddb.Application.Abstractions;
using Ddb.Application.Services;
using Ddb.Domain.Models;
using Ddb.Domain.Views;
using Ddb.Domain.Services;
using Ddb.Domain.Commands;
using Ddb.Domain.Enums;
using Ddb.Tests.Factories;

namespace Ddb.Tests.ApplicationTests
{
    public class ApplicationServiceTests
    {
        private readonly IApplicationService _instance;
        private readonly Mock<ICharacterRepository> _characterRepository;
        private readonly Mock<IEventBus> _eventBus;

        public ApplicationServiceTests()
        {
            // Create mocks of the dependencies
            _characterRepository = new Mock<ICharacterRepository>();
            _eventBus = new Mock<IEventBus>();

            // Create the character factory - Provide a seed to the DiceRollService to get deterministic results for testing.
            var diceRollService = new DiceRollService(1);
            var hitDiceService = new HitDiceService(diceRollService);
            var characterFactory = new CharacterFactory(hitDiceService);

            _instance = new ApplicationService(_characterRepository.Object, _eventBus.Object, characterFactory);
        }

        [Fact]
        public async Task GetCharacterAsync_ShouldReturnCharacterView()
        {
            // Arrange
            var id = Guid.NewGuid();
            var character = new Character("SomeName", new HitPoints(42));
            _characterRepository.Setup(x => x.GetByIdAsync(It.Is<Guid>(expected => expected == id)))
                .ReturnsAsync(character);
            // Act
            CharacterView result = await _instance.GetCharacterAsync(id);
            // Assert
            result.Name.Should().Be("SomeName");
            result.CurrentHp.Should().Be(42);
        }

        [Fact]
        public async Task CreateCharacterAsync_ShouldMakeCharacterAndReturnCharacterView()
        {
            // Arrange
            var command = CreateCharacterCommandFactory.SimpleCharacter();
            _characterRepository.Setup(x => x.SaveAsync(It.IsAny<Character>()));
            _eventBus.Setup(x => x.PublishAsync<CharacterView>(It.IsAny<CharacterView>()));
            // Act
            CharacterView result = await _instance.CreateCharacterAsync(command);
            // Assert
            _characterRepository.Verify(x => x.SaveAsync(It.IsAny<Character>()), Times.Once);
            _eventBus.Verify(x => x.PublishAsync<CharacterView>(It.Is<CharacterView>(expected => expected == result)), Times.Once);
            result.Name.Should().Be(command.Name);
            result.CurrentHp.Should().Be(22); // Same as in CharacterFactoryTests.cs
        }

        [Fact]
        public async Task AddTemporaryHpAsync_ShouldAddTempHpAndReturnCharacterView()
        {
            // Arrange
            var id = Guid.NewGuid();
            var character = new Character("SomeName", new HitPoints(42));
            var temporaryHp = new AddTemporaryHp { TemporaryHp = 5 };
            _characterRepository.Setup(x => x.GetByIdAsync(It.Is<Guid>(expected => expected == id)))
                .ReturnsAsync(character);
            _characterRepository.Setup(x => x.SaveAsync(It.IsAny<Character>()));
            _eventBus.Setup(x => x.PublishAsync<CharacterView>(It.IsAny<CharacterView>()));
            // Act
            CharacterView result = await _instance.AddTemporaryHpAsync(id, temporaryHp);
            // Assert
            _characterRepository.Verify(x => x.SaveAsync(It.Is<Character>(expected => expected == character)), Times.Once);
            _eventBus.Verify(x => x.PublishAsync<CharacterView>(It.Is<CharacterView>(expected => expected == result)), Times.Once);
            result.CurrentHp.Should().Be(42);
            result.TemporaryHp.Should().Be(5);
        }

        [Fact]
        public async Task HealHpAsync_ShouldHealHpAndReturnCharacterView()
        {
            // Arrange
            var id = Guid.NewGuid();
            var character = new Character("SomeName", new HitPoints(42));
            character.Hp.CurrentHp = 30;
            var healingHp = new HealHp { Hp = 5 };
            _characterRepository.Setup(x => x.GetByIdAsync(It.Is<Guid>(expected => expected == id)))
                .ReturnsAsync(character);
            _characterRepository.Setup(x => x.SaveAsync(It.IsAny<Character>()));
            _eventBus.Setup(x => x.PublishAsync<CharacterView>(It.IsAny<CharacterView>()));
            // Act
            CharacterView result = await _instance.HealHpAsync(id, healingHp);
            // Assert
            _characterRepository.Verify(x => x.SaveAsync(It.Is<Character>(expected => expected == character)), Times.Once);
            _eventBus.Verify(x => x.PublishAsync<CharacterView>(It.Is<CharacterView>(expected => expected == result)), Times.Once);
            result.CurrentHp.Should().Be(35);
        }

        [Fact]
        public async Task DealDamageAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var character = new Character("SomeName", new HitPoints(42));
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 1},
                new DealDamage {DamageType = DamageTypes.Cold, Hp = 2}
            };
            _characterRepository.Setup(x => x.GetByIdAsync(It.Is<Guid>(expected => expected == id)))
                .ReturnsAsync(character);
            _characterRepository.Setup(x => x.SaveAsync(It.IsAny<Character>()));
            _eventBus.Setup(x => x.PublishAsync<CharacterView>(It.IsAny<CharacterView>()));
            // Act
            CharacterView result = await _instance.DealDamageAsync(id, damages);
            // Assert
            _characterRepository.Verify(x => x.SaveAsync(It.Is<Character>(expected => expected == character)), Times.Once);
            _eventBus.Verify(x => x.PublishAsync<CharacterView>(It.Is<CharacterView>(expected => expected == result)), Times.Once);
            result.CurrentHp.Should().Be(39);
        }
    }
}
