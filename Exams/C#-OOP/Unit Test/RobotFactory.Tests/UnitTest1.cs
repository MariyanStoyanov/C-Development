using NUnit.Framework;
using System.Collections.Generic;

namespace RobotFactory.Tests
{
    public class Tests
    {
        private string name = "Ivan";
        private int capacity = 10;
        private string model = "Honda";
        private double price = 2.20;
        private int interfaceStandart = 100;
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Factory factory = new(name , capacity);
            Assert.AreEqual(name, factory.Name);
            Assert.AreEqual(capacity, factory.Capacity);
        }
        [Test]
        public void test2()
        {
            Factory factory = new(name, capacity);
            factory.ProduceRobot(model,price,interfaceStandart);
            Assert.AreEqual(1, factory.Robots.Count);
        }
        [Test]
        public void test123()
        {
            Factory factory = new(name, 3);
            factory.ProduceRobot(model, price, interfaceStandart);
            factory.ProduceRobot(model, price, interfaceStandart);
            factory.ProduceRobot(model, price, interfaceStandart);
            Robot robot = factory.Robots[0];
            Assert.AreEqual($"The factory is unable to produce more robots for this production day!", factory.ProduceRobot(model, price, interfaceStandart));
        }
        [Test]
        public void test1234()
        {
            Factory factory = new(name, 4);
            factory.ProduceRobot(model, price, interfaceStandart);
            factory.ProduceRobot(model, price, interfaceStandart);
            factory.ProduceRobot(model, price, interfaceStandart);
            Robot robot = factory.Robots[0];
            Assert.AreEqual($"Produced --> {robot}", factory.ProduceRobot(model, price, interfaceStandart));
        }
        [Test]
        public void test3()
        {
            Factory factory = new(name, capacity);
            factory.ProduceSupplement(model, interfaceStandart);
            Assert.AreEqual(1, factory.Supplements.Count);
        }
        [Test]
        public void test4()
        {
            Factory factory = new(name, capacity);
            Robot robot = new Robot(name,price,interfaceStandart);
            Supplement supplement = new Supplement(name,interfaceStandart);
            factory.Robots.Add(robot);
            factory.Supplements.Add(supplement);
            factory.UpgradeRobot(robot,supplement);
            Assert.AreEqual(1,robot.Supplements.Count);
        }
        [Test]
        public void test5()
        {
            Robot robot = new Robot(name, price, interfaceStandart);
            Robot robot2 = new Robot(name, price + 1, interfaceStandart);
            Factory factory = new(name, capacity);
            factory.Robots.Add(robot);
            factory.Robots.Add(robot2);
            Robot robot3 = factory.SellRobot(2.20);
            Assert.AreEqual(robot,robot3);
        }
        [Test]
        public void Test6()
        {
            Factory factory = new(name, capacity);
            factory.ProduceRobot(name,price,interfaceStandart);
            Assert.AreEqual(name,factory.Robots[0].Model);
            Assert.AreEqual(price, factory.Robots[0].Price);
            Assert.AreEqual(interfaceStandart, factory.Robots[0].InterfaceStandard);
        }
        [Test]
        public void Test7()
        {
            Robot robot = new(name, price, interfaceStandart);
            string text = robot.ToString();
            Assert.AreEqual(text,$"Robot model: {robot.Model} IS: {robot.InterfaceStandard}, Price: {robot.Price:f2}");
        }
        [Test]
        public void Test8()
        {
            Supplement supplement = new(name, interfaceStandart);
            string text = supplement.ToString();
            Assert.AreEqual(text, $"Supplement: {supplement.Name} IS: {supplement.InterfaceStandard}");
        }
        [Test]
        public void Test9()
        {
            Factory fact = new(name, capacity);
            Robot robot = new Robot(name,price,interfaceStandart);
            Supplement suppliment = new Supplement(name,interfaceStandart);
            fact.Robots.Add(robot);
            fact.Supplements.Add(suppliment);
            Assert.IsTrue(fact.UpgradeRobot(robot,suppliment));
            Assert.IsFalse(fact.UpgradeRobot(robot, suppliment));
        }
        [Test]
        public void Test10()
        {
            Factory fact = new(name, capacity);
            Robot robot = new Robot(name, price, interfaceStandart);
            fact.Robots.Add(robot);
            Assert.AreEqual(robot,fact.SellRobot(2.20));
        }
    }
}