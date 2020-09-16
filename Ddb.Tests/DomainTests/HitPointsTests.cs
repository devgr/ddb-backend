using System;
using Xunit;
using FluentAssertions;
using Ddb.Domain.Models;
using Ddb.Domain.Enums;
using Ddb.Domain.Commands;

namespace Ddb.Tests.DomainTests
{
    public class HitPointsTests
    {
        [Fact]
        public void Constructor_ShouldInitializeValues()
        {
            // Arrange and Act
            var instance = new HitPoints(42);

            // Assert
            instance.CurrentHp.Should().Be(42);
            instance.TemporaryHp.Should().Be(0);
            instance.MaxHp.Should().Be(42);
            instance.Status.Should().Be(LifeStatus.Stable);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.DeathSavingThrowSuccesses.Should().Be(0);
            instance.Immunities.Count.Should().Be(0);
            instance.Resistances.Count.Should().Be(0);
            instance.Vulnerabilities.Count.Should().Be(0);
        }

        [Fact]
        public void AddImmunity_ShouldAddDamageTypeToList()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddImmunity(DamageTypes.Fire);
            // Assert
            instance.Immunities.Count.Should().Be(1);
            instance.Immunities[0].Should().Be(DamageTypes.Fire);
        }

        [Fact]
        public void AddResistance_ShouldAddDamageTypeToList()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddResistance(DamageTypes.Fire);
            // Assert
            instance.Resistances.Count.Should().Be(1);
            instance.Resistances[0].Should().Be(DamageTypes.Fire);
        }

        [Fact]
        public void AddVulnerability_ShouldAddDamageTypeToList()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddVulnerability(DamageTypes.Fire);
            // Assert
            instance.Vulnerabilities.Count.Should().Be(1);
            instance.Vulnerabilities[0].Should().Be(DamageTypes.Fire);
        }

        [Fact]
        public void AddDeathSavingThrowFailure_ShouldIncrementCount()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddDeathSavingThrowFailure();
            // Assert
            instance.DeathSavingThrowFailures.Should().Be(1);
        }

        [Fact]
        public void AddDeathSavingThrowFailure_ShouldChangeStatusToDead_WhenCalledThreeTimes()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddDeathSavingThrowFailure();
            instance.AddDeathSavingThrowFailure();
            instance.AddDeathSavingThrowFailure();
            // Assert
            instance.DeathSavingThrowFailures.Should().Be(3);
            instance.Status.Should().Be(LifeStatus.Dead);
        }

        [Fact]
        public void AddTemporaryHp_ShouldSetTemporaryHp()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddTemporaryHp(5);
            // Assert
            instance.TemporaryHp.Should().Be(5);
        }

        [Fact]
        public void AddTemporaryHp_ShouldSetHigherTemporyHp_WhenCalledAgain()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddTemporaryHp(5);
            instance.AddTemporaryHp(7);
            // Assert
            instance.TemporaryHp.Should().Be(7);
        }

        [Fact]
        public void AddTemporaryHp_ShouldKeepHigherTemporyHp_WhenCalledAgainWithLowerNumber()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddTemporaryHp(9);
            instance.AddTemporaryHp(2);
            // Assert
            instance.TemporaryHp.Should().Be(9);
        }

        [Fact]
        public void AddTemporaryHp_ShouldDoNothing_WhenDead()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.Status = LifeStatus.Dead;
            // Act
            instance.AddTemporaryHp(5);
            // Assert
            instance.TemporaryHp.Should().Be(0);
        }

        [Fact]
        public void HealHp_ShouldAddHp_WhenLessThanMax()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 10;
            // Act
            instance.HealHp(5);
            // Assert
            instance.CurrentHp.Should().Be(15);
        }

        [Fact]
        public void HealHp_ShouldHealUpToMax_WhenMoreThanMax()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 10;
            // Act
            instance.HealHp(100);
            // Assert
            instance.CurrentHp.Should().Be(42);
        }

        [Fact]
        public void HealHp_ShouldStabilizeCharacter_WhenDying()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 0;
            instance.Status = LifeStatus.Dying;
            instance.DeathSavingThrowFailures = 2;
            instance.DeathSavingThrowSuccesses = 1;
            // Act
            instance.HealHp(3);
            // Assert
            instance.CurrentHp.Should().Be(3);
            instance.Status.Should().Be(LifeStatus.Stable);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.DeathSavingThrowSuccesses.Should().Be(0);
        }

        [Fact]
        public void HealHp_ShouldDoNothing_WhenDead()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 0;
            instance.Status = LifeStatus.Dead;
            instance.DeathSavingThrowFailures = 3;
            instance.DeathSavingThrowSuccesses = 1;
            // Act
            instance.HealHp(3);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.Status.Should().Be(LifeStatus.Dead);
            instance.DeathSavingThrowFailures.Should().Be(3);
            instance.DeathSavingThrowSuccesses.Should().Be(1);
        }

        [Fact]
        public void DealDamages_ShouldSubtractHp()
        {
            // Arrange
            var instance = new HitPoints(42);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 5},
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(37);
        }
    }
}
