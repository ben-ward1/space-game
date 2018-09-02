﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceGame
{
    public class Ship
    {
        // TODO CargoCapacity field exists for both Ship and Cargo classes. Trading calculations use Cargo's, but capacity will need
        // to change depending on ship upgrade in the future. Figure out how this is going to work or remove Ship.CargoCapacity.
        private int CargoCapacity = 1000;
        private int Fuel = 1000;
        private int FuelCapacity = 1000;
        private string ShipName = "Your Starter Ship";
        public Planet Location = Planet.Earth();
        private Cargo Cargo = new Cargo();

        private Planet Earth = new Planet();
        private Planet AlphaCentauri = new Planet(0, -4.367, "Alpha Centauri", 1, 2);
        private Planet MysteryPlanet = new Planet(-5, 4, "Mystery Planet", 2, 3);
        private Planet ShipGarage = new Planet(2, 1, "Easy Eddie's InterGalactic Garage and Massage Parlor", 3, 3);
        private Planet PizzaPlanet = new Planet(20, 30, "Pizza Planet", 3, 1);
        private Planet OtherPlanet = new Planet(-20, -30, "Other Planet", 3, 1);
        private Planet OtherPlanet2 = new Planet(-20, 30, "Other Planet2", 3, 1);
        private Planet OtherPlanet3 = new Planet(20, -30, "Other Planet3", 3, 1);
        private Planet OtherPlanet4 = new Planet(45, 45, "Other Planet4", 3, 1);

        // TODO is this the best place for this field?
        private double YearsLeft = 40;

        public Ship()
        {
            CargoCapacity = 1000;
            FuelCapacity = 1000;
            ShipName = "Your Starter Ship";
        }

        public Ship(int cargoCapacity, int fuelCapacity, string shipName, Planet location)
        {
            this.CargoCapacity = cargoCapacity;
            this.FuelCapacity = fuelCapacity;
            this.ShipName = shipName;
            this.Location = Planet.Earth();
        }

        public void Travel()
        {
            Planet destination = PickPlanet();
            // prompts user to input warpFactor.
            int warpFactor = WarpSelector();

            double fuelEfficiency = FuelEfficiencyCalc(warpFactor);
            double warpSpeed = Math.Pow(warpFactor, (10.0 / 3.0)) + Math.Pow((10 - warpFactor), (-11.0 / 3.0));

            // Resource burn calculations. Will be used to offer final travel decision to user.
            double fuelReq = Location.DistanceToPlanet(destination) * fuelEfficiency;
            double yearsReq = Location.DistanceToPlanet(destination) / (warpSpeed);


            // User confirmation based on fuel/time required for trip.
            bool decision = ConfirmTravel(yearsReq, fuelReq);

            if (decision == true)
            {
                // Subtracts fuel based on distance. fuelEfficiency increases (greater fuel reduction) with warpFactor.
                this.Fuel -= Convert.ToInt32(Location.DistanceToPlanet(destination) * fuelEfficiency);

                // Calculations for deducting time off game clock. Deduction increases with warpFactor.
                this.YearsLeft -= Location.DistanceToPlanet(destination) / (warpSpeed);

                this.Location = destination;

                Console.Clear();
                Console.WriteLine($"Welcome to {destination.PlanetName}.");
                Console.WriteLine();
                Console.WriteLine($"         Fuel = {this.Fuel}/{this.FuelCapacity}");
                Console.WriteLine($"Time traveled = {(40 - YearsLeft).ToString("F2")} years");
                Console.WriteLine();
            }
            else
            {
                Console.Clear();
            }
        }

        // selects destination when traveling
        private Planet PickPlanet()
        {
            Console.Clear();
            Console.WriteLine("Where would you like to go?");
            Console.WriteLine("1 = Earth");
            Console.WriteLine("2 = Alpha Centauri");
            Console.WriteLine("3 = Mystery Planet");
            Console.WriteLine("4 = Easy Eddie's InterGalactic Garage and Massage Parlor");
            Console.WriteLine();

            Planet destination = new Planet();

            int option = int.Parse(Console.ReadLine());

            switch (option)
            {
                case 1:
                    destination = Earth;
                    break;
                case 2:
                    destination = AlphaCentauri;
                    break;
                case 3:
                    destination = MysteryPlanet;
                    break;
                case 4:
                    destination = ShipGarage;
                    break;
                default:
                    Console.WriteLine("You must pick a valid destination.");
                    break;
            }
            return destination;
        }

        // Displays travel requirements, user confirms travel.
        private bool ConfirmTravel(double yearsReq, double fuelReq)
        {
            Console.WriteLine($"This trip will leave you with:");
            Console.WriteLine($"          Fuel = {(this.Fuel - fuelReq).ToString("F0")}/{this.FuelCapacity}");
            Console.WriteLine($"Years traveled = {(40 - this.YearsLeft + yearsReq).ToString("F2")} years");
            //Console.WriteLine($"This trip will take {yearsReq} years and cost {fuelReq} fuel.");
            Console.WriteLine();
            Console.WriteLine("Would you like to proceed?");
            Console.WriteLine("1 = Yes");
            Console.WriteLine("2 = No");
            Console.WriteLine();

            int option = Int32.Parse(Console.ReadLine());

            if (option == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int WarpSelector()
        {
            Console.WriteLine();
            Console.WriteLine("Select your warp speed, Captain.");
            Console.WriteLine("Enter a number 1 - 9.");
            Console.WriteLine();
            try
            {
                int warpFactor = 1;
                warpFactor = int.Parse(Console.ReadLine());
                Console.WriteLine();

                // ensures input is in range and valid.
                if (warpFactor < 1 && warpFactor > 9)
                {
                    Console.Clear();
                    Console.WriteLine("You must enter a valid warp speed.");
                    Console.WriteLine();
                    WarpSelector();
                }

                return warpFactor;
            }
            catch (Exception)
            {
                MainError();
                WarpSelector();
                // TODO is there a way to do this block without returning a value?
                return 1;
            }
        }

        // return value is used to increase fuel required as warpFactor increases.
        // TODO tune calculation when implementing fuel requirements as stated in requirements.md.
        private double FuelEfficiencyCalc(int warpFactor)
        {
            double efficiency = Math.Pow(warpFactor, 1.5);
            return efficiency;
        }

        public string GetLocation()
        {
            return Location.PlanetName;
        }

        public string GetShipName() => ShipName;

        public int GetItemID()
        {
            return Location.ItemID;
        }

        internal int GetCargoCapacity()
        {
            return Cargo.GetCargoCapacity();
        }

        public void ChangeCargoCapacity(int cargoCapacity)
        {
            Cargo.ChangeCargoCapacity(cargoCapacity);
        }

        internal int GetCargoWeight()
        {
            return Cargo.GetCargoWeight();
        }

        internal void ChangeCredits(int purchasePrice)
        {
            Cargo.ChangeCredits(purchasePrice);
        }

        internal void ChangeItem(int itemID, int quantity)
        {
            Cargo.ChangeItem(itemID, quantity);
        }

        internal void ChangeWeight(int weight)
        {
            Cargo.ChangeWeight(weight);
        }

        public int GetPlanetID()
        {
            return Location.PlanetID;
        }

        public int GetFuelLevel()
        {
            return this.Fuel;
        }

        public int GetFuelCapacity()
        {
            return this.FuelCapacity;
        }

        public int GetCredits()
        {
            return Cargo.GetCredits();
        }

        public double GetYears() => YearsLeft;

        public void CheckCargo()
        {
            Cargo.CheckCargo();
        }

        public void ChangeFuelCapacity(int fuelCapacity)
        {
            FuelCapacity = fuelCapacity;
        }

        // argument is positive when buying fuel, negative when traveling.
        public void ChangeFuel(int fuel)
        {
            this.Fuel += fuel;

            // enforces fuel capacity
            if (this.Fuel > this.FuelCapacity)
            {
                this.Fuel = this.FuelCapacity;
            }
        }

        public static Ship GetMyShip()
        {
            return new Ship(0, 0, "Your Starter Ship", Planet.Earth());
        }

        private void MainError()
        {
            Console.WriteLine();
            Console.WriteLine("Invalid Input");
            Console.WriteLine();
        }

        internal int GetItemQuant(int itemID)
        {
            return Cargo.GetItemQuant(itemID);
        }

        public void ChangeTotalEarned(int v)
        {
            Cargo.ChangeTotalEarned(v);
        }

        internal void ChangeShipName(string shipName)
        {
            ShipName = shipName;
        }

        public int GetPrice(int modifier)
        {
            return Earth.GetPrice(GetPlanetID() + modifier);
        }
    }
}
