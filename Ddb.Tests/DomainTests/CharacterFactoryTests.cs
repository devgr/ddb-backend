using System;
using System.IO;
using Xunit;
using FluentAssertions;
using Ddb.Domain.Models;
using Ddb.Domain.Enums;
using Ddb.Domain.Commands;
using Ddb.Domain.Services;

namespace Ddb.Tests.DomainTests
{
    public class CharacterFactoryTests
    {
        private readonly CharacterFactory _instance;
        public CharacterFactoryTests()
        {
            // Providing a seed to the DiceRollService to get deterministic results for testing.
            var diceRollService = new DiceRollService(1);
            var hitDiceService = new HitDiceService(diceRollService);
            _instance = new CharacterFactory(hitDiceService);
        }

        [Fact]
        public void BuildCharacter_ShouldCalculateInitialHitPoints_WhenSingleClass()
        {
            // Arrange
            var command = CreateCharacterCommandFactory.SimpleCharacter();
            // Act
            var character = _instance.BuildCharacter(command);
            // Assert
            character.Hp.CurrentHp.Should().Be(22);
            character.Name.Should().Be(command.Name);
        }

        [Fact]
        public void BuildCharacter_ShouldCalculateInitialHitPoints_WhenConstitutionPlus1()
        {
            // Arrange
            var command = CreateCharacterCommandFactory.SimpleCharacter(12);
            // Act
            var character = _instance.BuildCharacter(command);
            // Assert
            character.Hp.CurrentHp.Should().Be(25);
            character.Name.Should().Be(command.Name);
        }

        [Fact]
        public void BuildCharacter_ShouldCalculateInitialHitPoints_WhenItemConstitutionBonus()
        {
            // Arrange
            var command = CreateCharacterCommandFactory.CharacterWithItemConstitutionBonus();
            // Act
            var character = _instance.BuildCharacter(command);
            // Assert
            character.Hp.CurrentHp.Should().Be(25);
            character.Name.Should().Be(command.Name);
        }

        [Fact]
        public void BuildCharacter_ShouldCalculateInitialHitPoints_WhenMultiClass()
        {
            // Arrange
            var command = CreateCharacterCommandFactory.MultiClassCharacter();
            // Act
            var character = _instance.BuildCharacter(command);
            // Assert
            character.Hp.CurrentHp.Should().Be(87);
            character.Name.Should().Be(command.Name);
        }

        [Fact]
        public void BuildCharacter_ShouldSetDamageModififiers()
        {
            // Arrange
            var command = CreateCharacterCommandFactory.CharacterWithDamageModifiers();
            // Act
            var character = _instance.BuildCharacter(command);
            // Assert
            character.Hp.Immunities.Should().HaveCount(1);
            character.Hp.Immunities.Should().Contain(DamageTypes.Lightning);
            character.Hp.Resistances.Should().HaveCount(2);
            character.Hp.Resistances.Should().OnlyContain(x => x == DamageTypes.Fire);
            character.Hp.Vulnerabilities.Should().HaveCount(1);
            character.Hp.Vulnerabilities.Should().Contain(DamageTypes.Cold);
        }
    }
}
