using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        private Game game;

        [SetUp]
        public void Setup()
        {
            GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            game = gameGameObject.GetComponent<Game>();
        }  

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(game.gameObject);
        }


        [UnityTest]
        public IEnumerator AsteroidsMoveDown()
        {
            //create an asteroid to keep track
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            //get intitial pos y of asteroid
            float initialYPos = asteroid.transform.position.y;
            //wait for asteroid to move
            yield return new WaitForSeconds(0.1f);
            //check position
            Assert.Less(asteroid.transform.position.y, initialYPos);
        }

        [UnityTest]
        public IEnumerator GameOverOccursOnAsteroidCollision()
        {
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            //force the asteroid and ship to have to same position
            asteroid.transform.position = game.GetShip().transform.position;
            //wait to make sure Physics engine Collision is triggered
            yield return new WaitForSeconds(0.1f);
            //check if game is over
            Assert.True(game.isGameOver);
        }

        [UnityTest]
        public IEnumerator LaserMovesUp()
        {
            GameObject laser = game.GetShip().SpawnLaser();
            float initialYPos = laser.transform.position.y;
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(laser.transform.position.y, initialYPos);
        }

        [UnityTest]
        public IEnumerator LaserDestroysAsteroid()
        {
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            asteroid.transform.position = Vector3.zero;
            GameObject laser = game.GetShip().SpawnLaser();
            laser.transform.position = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            UnityEngine.Assertions.Assert.IsNull(asteroid);
        }

        [UnityTest]
        public IEnumerator NewGameRestartsGame()
        {
            game.isGameOver = true;
            game.NewGame();
            Assert.False(game.isGameOver);
            yield return null;
        }

        [UnityTest]
        public IEnumerator DestroyedAsteroidRaisesScore()
        {
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            asteroid.transform.position = Vector3.zero;
            GameObject laser = game.GetShip().SpawnLaser();
            laser.transform.position = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(game.score, 1);
        }

        [UnityTest]
        public IEnumerator NewGameScore0()
        {
            game.NewGame();
            Assert.AreEqual(0,game.score);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShipMoveLeft()
        {
            Ship ship = game.GetShip();
            float initialXPos= ship.transform.position.x;
            ship.MoveLeft();
            Assert.Less(ship.transform.position.x,initialXPos);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShipMoveRight()
        {
            Ship ship = game.GetShip();
            float initialXPos= ship.transform.position.x;
            ship.MoveRight();
            Assert.Greater(ship.transform.position.x,initialXPos);
            yield return null;
        }

        [UnityTest]
        public IEnumerator ShipIsDeadWhenExplode()
        {
            Ship ship = game.GetShip();
            ship.Explode();
            Assert.True(ship.isDead);
            yield return null;
        }

        
    }
}
