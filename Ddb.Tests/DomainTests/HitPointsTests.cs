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
            instance.Status.Should().Be(LifeStatuses.Stable);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.DeathSavingThrowSuccesses.Should().Be(0);
            instance.Immunities.Should().HaveCount(0);
            instance.Resistances.Should().HaveCount(0);
            instance.Vulnerabilities.Should().HaveCount(0);
        }

        [Fact]
        public void AddImmunity_ShouldAddDamageTypeToList()
        {
            // Arrange
            var instance = new HitPoints(42);
            // Act
            instance.AddImmunity(DamageTypes.Fire);
            // Assert
            instance.Immunities.Should().HaveCount(1);
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
            instance.Resistances.Should().HaveCount(1);
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
            instance.Vulnerabilities.Should().HaveCount(1);
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
            instance.Status.Should().Be(LifeStatuses.Dead);
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
            instance.Status = LifeStatuses.Dead;
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
            instance.Status = LifeStatuses.Dying;
            instance.DeathSavingThrowFailures = 2;
            instance.DeathSavingThrowSuccesses = 1;
            // Act
            instance.HealHp(3);
            // Assert
            instance.CurrentHp.Should().Be(3);
            instance.Status.Should().Be(LifeStatuses.Stable);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.DeathSavingThrowSuccesses.Should().Be(0);
        }

        [Fact]
        public void HealHp_ShouldDoNothing_WhenDead()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 0;
            instance.Status = LifeStatuses.Dead;
            instance.DeathSavingThrowFailures = 3;
            instance.DeathSavingThrowSuccesses = 1;
            // Act
            instance.HealHp(3);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.Status.Should().Be(LifeStatuses.Dead);
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

        [Fact]
        public void DealDamages_ShouldSumAllDamage_WhenMultipleDamages()
        {
            // Arrange
            var instance = new HitPoints(42);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 1},
                new DealDamage {DamageType = DamageTypes.Cold, Hp = 2},
                new DealDamage {DamageType = DamageTypes.Necrotic, Hp = 3},
                new DealDamage {DamageType = DamageTypes.Psychic, Hp = 4},
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(32);
        }

        [Fact]
        public void DealDamages_ShouldDoNoDamage_WhenImmunity()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddImmunity(DamageTypes.Fire);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 5}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(42);
        }

        [Fact]
        public void DealDamages_ShouldDoHalfDamageRoundedDown_WhenResitance()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddResistance(DamageTypes.Fire);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 5}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(40);
        }

        [Fact]
        public void DealDamages_ShouldDoDoubleDamage_WhenVulnerability()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddVulnerability(DamageTypes.Fire);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 5}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(32);
        }

        [Fact]
        public void DealDamages_ShouldOnlyDoHalfDamage_WhenMultipleResistances()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddResistance(DamageTypes.Fire);
            instance.AddResistance(DamageTypes.Fire);
            instance.AddResistance(DamageTypes.Fire);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 8}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(38);
        }

        [Fact]
        public void DealDamages_ShouldProcessResistanceThenVulnerability_WhenBoth()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddResistance(DamageTypes.Fire);
            instance.AddVulnerability(DamageTypes.Fire);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 9}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(34);
        }

        [Fact]
        public void DealDamages_ShouldUseTemporaryHPOnly_WhenEnough()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddTemporaryHp(5);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 5}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(42);
            instance.TemporaryHp.Should().Be(0);
        }

        [Fact]
        public void DealDamages_ShouldUseTemporaryHPThenCurrentHP_WhenNotEnough()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.AddTemporaryHp(5);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 10}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(37);
            instance.TemporaryHp.Should().Be(0);
        }

        [Fact]
        public void DealDamages_ShouldIncrememntFailures_WhenDying()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 0;
            instance.Status = LifeStatuses.Dying;
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 5}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.DeathSavingThrowFailures.Should().Be(1);
        }

        [Fact]
        public void DealDamages_ShouldChangeStatusToDead_WhenDyingAndDamageIsMaxOrMore()
        {
            // Arrange
            var instance = new HitPoints(42);
            instance.CurrentHp = 0;
            instance.Status = LifeStatuses.Dying;
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 42}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.Status.Should().Be(LifeStatuses.Dead);
        }

        [Fact]
        public void DealDamages_ShouldChangeStatusToDying_WhenStableAndDamageIsSameAsCurrent()
        {
            // Arrange
            var instance = new HitPoints(42);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 42}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.Status.Should().Be(LifeStatuses.Dying);
        }

        [Fact]
        public void DealDamages_ShouldNotHaveNegativeHP_WhenDamageIsMoreThanCurrent()
        {
            // Arrange
            var instance = new HitPoints(42);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 45}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.Status.Should().Be(LifeStatuses.Dying);
        }

        [Fact]
        public void DealDamages_ShouldChangeStatusToDead_WhenRemainingDamageIsMaxOrMore()
        {
            // Arrange
            var instance = new HitPoints(42);
            var damages = new DealDamage[]
            {
                new DealDamage {DamageType = DamageTypes.Fire, Hp = 84}
            };
            // Act
            instance.DealDamages(damages);
            // Assert
            instance.CurrentHp.Should().Be(0);
            instance.DeathSavingThrowFailures.Should().Be(0);
            instance.Status.Should().Be(LifeStatuses.Dead);
        }
    }
}
