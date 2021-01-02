using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#region Gaz Comments
// A simple First Person Character controller that works /mostly/ outside the physics system
// Builds upon the built in CharacterController class so that it will interact nicely with collisions
#endregion
//Input System integration based on https://www.youtube.com/watch?v=vbILVirFV3A, 
//https://www.youtube.com/watch?v=p-3S73MaDP8 and 
//https://www.youtube.com/watch?v=HwbbvjzT3qE.

//I've region'd out Gaz' comments. Anything that's left visible should be mine.
//Hopefully easier to track my train of thought that way.
public class FPS_Player : MonoBehaviour
{
    #region Gaz' variables
    //A reference to the Player's CharacterController, so that we can control it and call functions later on
    CharacterController m_CharacterController;

    //What should the current velocity of our Player be?
    Vector3 m_Velocity;

    //[SerializeField] allows us to modify the following variable in the Unity Inspector
    //What is our desired maximum lateral movement speed of the player?
    [SerializeField] float m_Speed = 12.0f;

    //What is our desired maximum rotation speed of the player's camera (in degrees per second)?
    [SerializeField] float m_RotateSpeed = 360.0f;

    bool m_IsGrounded = false;

    //Yaw is our Horizontal rotation
    //Pitch is our vertical rotation
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    #endregion

    //I'm sorry in advance for what's about to happen when you expand this region.
    #region My Variables

    #region reticule display
    [SerializeField] private Image Reticule;
    [SerializeField] private Image Hitmarker;
    [SerializeField] private Image AmmoCounter;
    [SerializeField] private Image BoostDisplay;
    [SerializeField] private Sprite ReticuleTarget;
    [SerializeField] private Sprite ReticuleNoTarget;
    [SerializeField] private Sprite ReticuleKillTarget; //Hitmarkers permanently on this sprite. Mostly for trying out permutations.
    [SerializeField] private Sprite ReticuleSmallTarget; //The new "I can instantiate a heat volume here" sprite. I'd probably just remove this functionality entirely, now, if I wasn't using it for debug
    [SerializeField] private Sprite AmmoEmpty;
    [SerializeField] private Sprite AmmoOne;
    [SerializeField] private Sprite AmmoTwo;
    [SerializeField] private Sprite AmmoThree;
    [SerializeField] private Sprite BoostYes;
    [SerializeField] private Sprite BoostNo;
    [SerializeField] private GameObject uiDebug;
    private HasBeenShot currentTarget;
    private DebugText DebugText;
    Ray trajectoryCheck;
    RaycastHit targetPointCheck;
    bool fadeIn = false;
    bool fadeOut = false;
    float fadeInTime = 0.05f;
    float fadeOutTime = 0.4f;
    float hitMarkerTimer = 0.0f;
    bool scooch = false;
    bool scoochAccelerate = false;
    Color markerOpacity;
    #endregion

    #region bullet instantiation
    private HeatDissipate CurrentField;
    public GameObject bullet;
    public Vector3 instantiationPoint;
    private bool fireAgain = true;
    Ray trajectory;
    RaycastHit targetpoint;
    private int layerMask = ~(1 << 8); //Ignores anything tagged as a heat volume. This allows the weapon's hitscan to ignore certain objects.
    private int enemyMask = 1 << 10; //Searches specifically for navmesh actors, for the purposes of bullet magnetism and AoE detection.
    Vector3 AoeFinder;
    RaycastHit additionalHitscan;
    #endregion

    #region heat reserves
    public GameObject regenerator;
    private float heatMax = 100.0f;
    private float heatCost = 30.0f;
    public float heatGain = 0.0f;
    public float constantGain = 0.0f;
    public float heatCurrent;
    private float scoochCost = 20.0f;
    #endregion

    //Remade movement/aiming variables
    FPS_Controls controls;
    Vector2 move;
    Vector2 aim;
    Ray jumpRay; //This is no longer necessary, but I've left it in for posterity.
    RaycastHit groundTarget;
    [SerializeField] private float speedCap = 18.38f; //the square root of 13 squared, a little over the normal max velocity in any given direction.
    private int playerMask = ~(1 << 9); //Ignores the player, for the purposes of checking collision with the ground
    private float airTime;
    private bool canDouble = false;
    private float baseFov = 0;
    private float fovMax = 10.0f;
    #region aim smoothing
    private float smoothY;
    private float smoothX;
    private float smoothTime = 0.3f;
    #endregion
    #region movement smoothing
    Vector2 m_VelocityLast; //Records the player's last known velocity on the ground, for the purposes of determining airborne mobility.
    Vector2 m_VelocityAnchor; //This has been left in mostly to catalogue its failure, though it is also used during the boost.
    [SerializeField] private float anchorMax = 0.5f; //Determines the maximum acelleration per-frame, to prevent the player from shooting off sideways.
    Vector2 m_VelocityCurrent; //Component of airborne velocity directly under player control. Works like m_Velocity on the ground, functions as a component of m_Velocity in the air.
    private Vector2 m_ClampedVelocity; //This is used to determine the clamped horizontal component of the player's speed
    private float airDecay = 0.0f; //Determines what fraction of m_VelocityCurrent is used.
    private float airMax = 1.0f;
    private float decayMax = 0.0f; //Was 0.1f
    private float speedUp = 0.0f;
    private float speedMax = 0.6f; //Was 0.6f
    private float grace = 0.0f;
    private float graceMax = 0.04f;
    private bool hasStopped = false;
    private bool deadStop = false;
    private float stopTime = 0.0f;
    private float stopMax = 0.1f;
    private float airMult = 0.35f; //Was 0.3f
    private bool justLanded = false;
    private float landLerp = 0.0f;
    private float landTime = 0.0f;
    private float landMax = 0.3f;
    private float scoochTime = 0.0f;
    private float scoochAccMax = 0.1f;
    private float scoochDecMax = 0.3f;
    private float scoochAmp = 2.6f;
    #endregion
    #endregion

    private void Awake()
    {
        //Sets the player's health full, and their controls to the input system
        controls = new FPS_Controls();
        heatCurrent = heatMax;
        baseFov = Camera.main.fieldOfView;
    }



    // Start is called before the first frame update
    void Start()
    {
        #region Gaz' initialising
        //Find the reference to our Player's "CharacterController" component
        m_CharacterController = GetComponent<CharacterController>();
        #endregion
        //Gets the UI debug text, then sets the hit markers to be opaque and the initial m_VelocityAnchor to 0,0
        DebugText = uiDebug.GetComponent<DebugText>();
        markerOpacity = Hitmarker.color;
        markerOpacity.a = 0.0f;
        Hitmarker.color = markerOpacity;
        m_VelocityAnchor = new Vector2(0.0f, 0.0f);
    }

    #region Player movement
    public void MakeMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        //for determining when deceleration should begin, to smooth movement
        deadStop = false;
        //for determining when acceleration should begin, to smooth movement
        grace = 0.0f;
    }
    #endregion

    #region player aiming
    public void MakeAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
    }
    #endregion

    //The jump and boost mechanics are in here.
    #region player firing
    public void Firing(InputAction.CallbackContext context)
    {
        if (context.performed && fireAgain == true && heatCurrent >=30 )
        {
            CastRay();
            fireAgain = false;
        }
    }

    public void TriggerRelease(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            fireAgain = true;
        }
    }

    public void Jumping(InputAction.CallbackContext context)
    {
        //airTime puts jumping on a cooldown.
        //This prevents the player "double jumping" before the raycast function recognises they've left the ground.
        //Or being able to rapidly hammer jump up inclines to level Athletics, <i>Morrowind</i>.
        //As the calculation checking whether the player is airborne is now in update, it's not necessary here.
        //Whoops, wasn't actually checking the player was grounded for a while.
        if (context.performed && Time.time > airTime && m_IsGrounded == true)
        {
            airTime = Time.time + 0.3f;
            //This is temporary, might make it physics-based.
            m_Velocity.y = 5.0f;
            canDouble = true;
        }
        //Thing is, the first few months I wasn't checking m_IsGrounded, I'd been using what I assumed was a quirk in how the jump was programmed to
        //jump again in mid-air as a sort-of brake.
        //This is now a feature.
        /*else if (context.performed && Time.time > airTime && canDouble == true)
        {
            //Doesn't matter that this isn't reset anywhere else. Once the player touches ground, the other jump is always tested first, anyway.
            canDouble = false;
            scooch = true;
            scoochAccelerate = true;
            scoochTime = 0.0f;
            m_Velocity.y = 5.0f;
            heatCurrent -= 12.0f;
        }*/
    }

    public void Boost(InputAction.CallbackContext context)
    {
        //Decided to rebind the boost to its own unique button, for more versatility.
        if (context.performed && scooch == false && heatCurrent >= scoochCost)
        {
            canDouble = false;
            scooch = true;
            scoochAccelerate = true;
            scoochTime = 0.0f;
            m_Velocity.y = 3.2f; //This one's not gonna give you as much height as the normal jump, I didn't want the player to be able to gain altitude
            heatCurrent -= scoochCost;
            airTime = Time.time + 0.3f; //To prevent jump weirdness

        }
    }
    #endregion


    void Update()
    {
        //Making sure the player's on the ground before I tackle anything else
        JumpRay();

        //I've made alterations to this
        // I've tweaked the inputs the analogue sticks are reading from, and replaced the jumping function
        //There's also significant updates to transitions between types of movement or to/from stopped
        //And a section on aerial mobility
        //A lot of this you don't really feel when playing, but you'd miss if it was gone.
        #region Gaz' Update
        #region Gaz Comment
        //Accellerate our Player due to the effects of gravity (note: this will apply even when on the ground!)
        //but only if we're not on the ground
        #endregion
        if (m_IsGrounded == false)
        {
            m_Velocity += Physics.gravity * Time.deltaTime;
        }

        //Primes aiming for smoothing if a direction is released
        //This makes the crosshairs less likely to jump over small targets when making small adjustments
        if (aim.y == 0.0f)
        {
            smoothY = 0.0f;
        }
        if (aim.x == 00.0f)
        {
            smoothX = 0.0f;
        }

        //Smooths aiming in the Y direction
        if (smoothY < smoothTime)
        {
            pitch -= aim.y * Time.deltaTime * Mathf.Lerp(0, m_RotateSpeed, smoothY / smoothTime);
            smoothY += Time.deltaTime;
        }
        else 
        {
            pitch -= aim.y * Time.deltaTime * m_RotateSpeed;
        }
        pitch = Mathf.Clamp(pitch, -89.0f, 89.0f);

        //Smooths aiming in the X direction
        if (smoothX < smoothTime)
        {
            yaw += aim.x * Time.deltaTime * Mathf.Lerp(0, m_RotateSpeed, smoothX / smoothTime);
            smoothX += Time.deltaTime;
        }
        else 
        {
            yaw += aim.x * Time.deltaTime * m_RotateSpeed;
        }

        #region Gaz Comment
        //Set the body's rotation
        //Quaternion is just a mechanism for managing rotations. You don't need to directly work with it.
        //You can just ask it to give you a rotation, which is what we're doing here
        //Give me a rotation of 'yaw' degress around the Y-axis, then apply it to the body
        #endregion
        transform.localRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        #region Gaz Comment
        //Then we do the same for the head pitch
        #endregion
        Camera.main.transform.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        #region Gaz Comment
        // Get some information about the camera.
        // Which way is it looking now? And which way does it consider "to the right"?
        #endregion
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        #region Gaz Comment
        // Remove the 'y' component from our "look at" direction.
        // We don't want the player to fly when they move forward!
        #endregion
        cameraForward.y = 0;
        #region Gaz Comment
        //Normalising the vector turns it into a 'unit vector'; This makes the 'length' of it equal '1'
        //This is a lesson to have outside of comments if you're curious :)
        #endregion
        cameraForward.Normalize();

        #region Gaz Comment
        //Here we are modifying our "look at" directions to give them
        // a speed based on the input of the left analogue stick
        // leftStickInputDirection
        #endregion
        Vector3 playerForwardMovement   = cameraForward * move.y * m_Speed;
        Vector3 playerSideMovement      = cameraRight * move.x * m_Speed;

        //If the player has released controls, sets them up to decelerate
        if (move.x == 0 && move.y == 0)
        {
            deadStop = true;
        }

        //If the player has stopped, primes the player to briefly accelerate to top speed.
        if (m_Velocity.x == 0 && m_Velocity.z == 0)
        {
            grace += Time.deltaTime;
        }

        if (grace >= graceMax)
        {
            if (hasStopped == false)
            {
                speedUp = 0.0f;
            }
            hasStopped = true;
        }

        #region Gaz Comment
        //Now we take these values and apply them to our current movement speed
        // We're now 'moving' along the Camera's forward and right directions
        #endregion
        #region Player movement on the ground
        //Handles player movement on the ground, inluding smoothed acceleration and deceleration
        if (m_IsGrounded == true)
        {
            //If the player just hit the ground after too long in the air
            if (justLanded == true)
            {
                landLerp = Mathf.Lerp(airMult, 1, landTime / landMax);
                m_Velocity.x = landLerp * (playerForwardMovement.x + playerSideMovement.x);
                m_Velocity.z = landLerp * (playerForwardMovement.z + playerSideMovement.z);
                landTime += Time.deltaTime;
                if (landTime > landMax)
                {
                    justLanded = false;
                    landTime = 0.0f;
                }
            }
            //If the player has stopped long enough to need to accelerate up to speed again
            if (hasStopped == true)
            {
                m_VelocityCurrent.x = Mathf.Lerp(0, 1, speedUp / speedMax);
                m_VelocityCurrent.y = Mathf.Lerp(0, 1, speedUp / speedMax);
                m_Velocity.x = m_VelocityCurrent.x * (playerForwardMovement.x + playerSideMovement.x);
                m_Velocity.z = m_VelocityCurrent.y * (playerForwardMovement.z + playerSideMovement.z);
                speedUp += Time.deltaTime;
                if (speedUp > speedMax)
                {
                    hasStopped = false;
                    speedUp = 0.0f;
                }
            }
            //motion if the player has released controls
            else if (deadStop == true)
            {
                m_Velocity.x = Mathf.Lerp(m_VelocityLast.x, 0, stopTime / stopMax);
                m_Velocity.z = Mathf.Lerp(m_VelocityLast.y, 0, stopTime / stopMax);
                stopTime += Time.deltaTime;
            }
            //Motion as normal
            else
            {
                m_Velocity.x = playerForwardMovement.x + playerSideMovement.x;
                m_Velocity.z = playerForwardMovement.z + playerSideMovement.z;
                m_VelocityLast.x = m_Velocity.x;
                m_VelocityLast.y = m_Velocity.z;
                stopTime = 0.0f;
            }

            airDecay = 0.0f;
        }
        #endregion

        #region Player movement in the air
        //If the player is airborne, momentum guides their motion, and is lost over time.
        if (m_IsGrounded == false)
        {
            //Lerps player motion from their full ground speed to a set value as they spend more time in the air, simulating loss of momentum.
            //AirMax is the total duration of this effect
            //AirMult is the fraction of total, ground-based speed remaining at the end of the effect.
            if (airDecay < airMax)
            {
                m_VelocityCurrent.x = Mathf.Lerp(m_VelocityLast.x, decayMax, airDecay / airMax);
                m_Velocity.x = m_VelocityCurrent.x + (airMult * playerForwardMovement.x) + (airMult * playerSideMovement.x);
                m_VelocityCurrent.y = Mathf.Lerp(m_VelocityLast.y, decayMax, airDecay / airMax);
                m_Velocity.z = m_VelocityCurrent.y + (airMult * playerForwardMovement.z) + (airMult * playerSideMovement.z);
                airDecay += Time.deltaTime;
            }
            else
            {
                justLanded = true;
                m_Velocity.x = (airMult * playerForwardMovement.x) + (airMult * playerSideMovement.x);
                m_Velocity.z = (airMult * playerForwardMovement.z) + (airMult * playerSideMovement.z);
            }
        }
        #endregion

        #region the Christmas Smoothening

        //Stops (or at least smooths) the player catapulting off in random directions under specific edge cases
        //Fixes the maximum acelleration to anchorMax/Frame;
        //This was, in hindsight, utter nonsense.
        #region The Christmas Nonsense
        /*if (Mathf.Abs(m_Velocity.x - m_VelocityAnchor.x) > anchorMax)
        {
            if (Mathf.Abs(m_Velocity.x - m_VelocityAnchor.x) == m_Velocity.x - m_VelocityAnchor.x)
            {
                //This means the player was accelerating towards a positive x value since last frame
                m_Velocity.x = m_VelocityAnchor.x + anchorMax;
            }
            else
            {
                //This means the player was accelerating towards a negative x value since last frame
                //m_Velocity.x = m_VelocityAnchor.x + anchorMax;
            }
        }
        if (Mathf.Abs(m_Velocity.z - m_VelocityAnchor.z) > anchorMax)
        {
            if (Mathf.Abs(m_Velocity.z - m_VelocityAnchor.z) == m_Velocity.z - m_VelocityAnchor.z)
            {
                m_Velocity.z = m_VelocityAnchor.z + anchorMax;
            }
            else
            {
                //m_Velocity.z = m_VelocityAnchor.z - anchorMax;
            }
        } */
        #endregion

        if (scooch == false)
        {
            //I got this from unity documentation
            //Functionally, it clamps the player's lateral motion, leaving jumping a free, independent action.
            //Clamped velocity shows up below, too.
            m_ClampedVelocity.x = m_Velocity.x;
            m_ClampedVelocity.y = m_Velocity.z;
            //For the life of me, I don't know why these two lines are messing with the wrong vector components of the player's motion
            //The intent here was to limit the difference in the player's speed between frames, to prevent spontaneous directional changes, as above.
            //m_ClampedVelocity.x = Mathf.Clamp(m_ClampedVelocity.x, m_VelocityAnchor.x - 0.5f, m_VelocityAnchor.x + 0.5f);
            //m_ClampedVelocity.y = Mathf.Clamp(m_ClampedVelocity.y, m_VelocityAnchor.y - 0.5f, m_VelocityAnchor.y + 0.5f);
            m_ClampedVelocity = Vector2.ClampMagnitude(m_ClampedVelocity, speedCap);
            m_Velocity.x = m_ClampedVelocity.x;
            m_Velocity.z = m_ClampedVelocity.y;

            m_VelocityAnchor.x = m_Velocity.x;
            m_VelocityAnchor.y = m_Velocity.z;
        }
        #endregion

        #region Boost mechanic
        if (scooch == true)
        {
            //All for the purposes of normalising the lateral component of the player's motion.
            //This lets me base the boost mechanics around the player's maximum possible speed.
            //After some experimentation, included the momentum calculation from normal jumping to prevent instantaneous directional shifts in the air.
            m_ClampedVelocity.x = m_VelocityAnchor.x;
            m_ClampedVelocity.y = m_VelocityAnchor.y;
            if (scoochAccelerate == true)
            {
                scoochTime += Time.deltaTime;
                m_VelocityAnchor = new Vector2(m_ClampedVelocity.x, m_ClampedVelocity.y).normalized * speedCap;
                if (scoochTime >= scoochAccMax)
                {
                    scoochAccelerate = false;
                    scoochTime = 0.0f;
                    m_Velocity.x = m_VelocityAnchor.x * scoochAmp;
                    m_Velocity.z = m_VelocityAnchor.y * scoochAmp;
                    Camera.main.fieldOfView = baseFov + fovMax;
                }
                else
                {
                    m_Velocity.x = m_VelocityAnchor.x * Mathf.Lerp(1, scoochAmp, scoochTime / scoochAccMax);
                    m_Velocity.z = m_VelocityAnchor.y * Mathf.Lerp(1, scoochAmp, scoochTime / scoochAccMax);
                    Camera.main.fieldOfView = baseFov + Mathf.Lerp(0, fovMax, scoochTime / scoochAccMax);
                }
            }
            else
            {
                scoochTime += Time.deltaTime;
                m_VelocityAnchor = new Vector2(m_ClampedVelocity.x + (0.01f * playerForwardMovement.x) + (0.01f * playerSideMovement.x), m_ClampedVelocity.y + (0.01f * playerForwardMovement.z) + (0.01f * playerSideMovement.z)).normalized * speedCap;
                if (scoochTime >= scoochDecMax)
                {
                    scoochAccelerate = true;
                    scooch = false;
                    scoochTime = 0.0f;
                    Camera.main.fieldOfView = baseFov;
                    m_VelocityLast = m_VelocityAnchor;
                    m_Velocity.x = m_VelocityAnchor.x;
                    m_Velocity.z = m_VelocityAnchor.y;
                    airDecay = 0.0f;
                    justLanded = false;
                }
                else
                {
                    m_Velocity.x = m_VelocityAnchor.x * Mathf.Lerp(scoochAmp, 1, scoochTime / scoochDecMax);
                    m_Velocity.z = m_VelocityAnchor.y * Mathf.Lerp(scoochAmp, 1, scoochTime / scoochDecMax);
                    Camera.main.fieldOfView = baseFov + Mathf.Lerp(fovMax, 0, scoochTime / scoochDecMax);
                }
            }
        }
        #endregion

        #region Gaz Comment
        //Finally tell the CharacterController to 'move' our body based on our velocity!
        // We multiply the value by "Delta time" to make the movement happen over time, rather than a huge speed per frame
        //This makes our movement framerate independant!
        #endregion
        m_CharacterController.Move(m_Velocity * Time.deltaTime);


        #region Gaz' old jump
        /*
        if (Input.GetButtonDown("Cross"))
        {
            //Jump
            //Debug.Log("Jump!"); //Print a message to the console! This is really helpful for debugging errors!
            m_Velocity.y = 5.0f; //To jump, we set our vertical velocity to a positive value, meaning we'll move up the way
            m_IsGrounded = false;
        }
        */
        #endregion
        #endregion

        #region passive heat regen
        heatCurrent += (5 * Time.deltaTime);
        heatCurrent += (constantGain * Time.deltaTime);
        heatCurrent = Mathf.Clamp(heatCurrent, 0, heatMax);
        //Debug.Log(heatCurrent);
        DebugText.uiText = heatCurrent;
        #endregion

        #region ammo counter display
        if (heatCurrent >= 90.0f)
        {
            AmmoCounter.sprite = AmmoThree;
        }
        else if (heatCurrent >= 60.0f)
        {
            AmmoCounter.sprite = AmmoTwo;
        }
        else if (heatCurrent >= 30.0f)
        {
            AmmoCounter.sprite = AmmoOne;
        }
        else
        {
            AmmoCounter.sprite = AmmoEmpty;
        }
        #endregion

        //Checks which reticule colouration to display
        CheckTarget();

        //Controls hitmarker fade
        #region hitmarker
        //Fades in the hitmarker
        if (fadeIn == true)
        {
            hitMarkerTimer += Time.deltaTime;
            if (hitMarkerTimer >= fadeInTime)
            {
                hitMarkerTimer = 0.0f;
                fadeIn = false;
                fadeOut = true;
            }
            else
            {
                markerOpacity.a = Mathf.Lerp(0, 1, hitMarkerTimer / fadeInTime);
                Hitmarker.color = markerOpacity;
            }
        }

        //Fades out the hitmarker
        if (fadeOut == true)
        {
            hitMarkerTimer += Time.deltaTime;
            if (hitMarkerTimer >= fadeOutTime)
            {
                hitMarkerTimer = 0.0f;
                fadeOut = false;
            }
            else
            {
                markerOpacity.a = Mathf.Lerp(1, 0, hitMarkerTimer / fadeOutTime);
                Hitmarker.color = markerOpacity;
            }
        }
        #endregion

        //Controls the display for whether or not you can boost
        #region boost display
        if (scooch == false && heatCurrent >= scoochCost)
        {
            BoostDisplay.sprite = BoostYes;
        }
        else
        {
            BoostDisplay.sprite = BoostNo;
        }
        #endregion

    }
    #region Gaz Comment
    //Here's a bonus function!
    // We can put code here that we want to trigger when our character touches something (anything right now!)
    #endregion

    private void CastRay()
    {
    #region weapon firing ray
        //Raycast for weapon targeting.
        trajectory = new Ray(transform.position + new Vector3(0.0f, 0.608f, 0.0f), Camera.main.transform.forward);
        Debug.DrawRay(trajectory.origin, trajectory.direction * 100, Color.blue, 2, false);
        fireAgain = false;
        if(Physics.Raycast(trajectory, out targetpoint, Mathf.Infinity, layerMask) && targetpoint.collider != null)
        {
            heatCurrent -= heatCost;
            instantiationPoint = targetpoint.point;
            //First checks to see if a navmesh actor was shot directly
            if (targetpoint.transform.gameObject.tag == "Foe")
            {
                Instantiate(bullet, instantiationPoint, Quaternion.identity);
                currentTarget = targetpoint.transform.gameObject.GetComponent<HasBeenShot>();
                currentTarget.hasBeenShot = true;
                fadeIn = true;
            }
            //using what I've learned from the ground detection spherecast, and the other raycasts I've used so far...
            else if (Physics.SphereCast(transform.position, 0.3f, Camera.main.transform.forward, out additionalHitscan, targetpoint.distance, enemyMask))
            {
                //I've created bullet magnetism.
                if (additionalHitscan.transform.gameObject.tag == "Foe")
                {
                    Debug.Log("Bullet Magnetism struck target");
                    Instantiate(bullet, additionalHitscan.point, Quaternion.identity);
                    currentTarget = additionalHitscan.transform.gameObject.GetComponent<HasBeenShot>();
                    currentTarget.hasBeenShot = true;
                    fadeIn = true;
                }
            }
            else
            {
                //Debug.Log("I hit something!");
                //Debug.Log("At " + instantiationPoint.x + ", " + instantiationPoint.y + ", " + instantiationPoint.z);
                if (targetpoint.transform.gameObject.tag == "Target")
                {
                    Instantiate(bullet, instantiationPoint, Quaternion.identity);
                    fadeIn = true;
                }
            }

            //This one's for AoE considerations, checks I haven't hit anything near the blast radius.
            //Advance Higher mechanics really pulling through for me, now.
            AoeFinder = instantiationPoint - (2.2f * new Vector3(instantiationPoint.x - transform.position.x, instantiationPoint.y - transform.position.y, instantiationPoint.z - transform.position.z).normalized);
            if (Physics.SphereCast(AoeFinder, 1.1f, Camera.main.transform.forward, out additionalHitscan, 2.2f, enemyMask))
            {
                if (additionalHitscan.transform.gameObject.tag == "Foe")
                {
                    //This will get flagged even if an enemy is shot directly
                    Debug.Log("Caught additional target");
                    currentTarget = additionalHitscan.transform.gameObject.GetComponent<HasBeenShot>();
                    currentTarget.hasBeenShot = true;
                }
            }

        }
        #endregion
    }

    private void CheckTarget()
    {
        #region target checking ray
        //Checks the object the reticule's over can instantiate volumes.
        //If it can, changes the reticule dot colour to red.
        //If it's a navmesh actor, sets the reticule ring to red.
        //otherwise, changes it to white.
        trajectoryCheck = new Ray(transform.position + new Vector3(0.0f, 0.608f, 0.0f), Camera.main.transform.forward);
        if (Physics.Raycast(trajectoryCheck, out targetPointCheck, Mathf.Infinity, layerMask) && targetPointCheck.collider != null)
        {
            if (targetPointCheck.transform.gameObject.tag == "Target")
            {
                Reticule.sprite = ReticuleSmallTarget;
            }
            else if(targetPointCheck.transform.gameObject.tag == "Foe")
            {
                Reticule.sprite = ReticuleTarget;
            }
            else
            {
                Reticule.sprite = ReticuleNoTarget;
            }
        }
        else
        {
            Reticule.sprite = ReticuleNoTarget;
        }
        #endregion
    }

    private void JumpRay()
    {
        #region Raycast (old)
        //More accurate calculation for when the player is touching the ground.
        //During tutorials, I realised the player cannot fall unless m_IsGrounded is false and this condition is only met after jumping.
        //So this allows the player to fall after walking over the ends of surfaces.
        //Now casts five rays, in the hope 
        /*jumpRay = new Ray(transform.position + new Vector3(0.0f, -1.0f, 0.0f), Vector3.down);
        Debug.DrawRay(jumpRay.origin, jumpRay.direction * 0.4f, Color.white, 2, false);
        if(Physics.Raycast(jumpRay, out groundTarget) && groundTarget.collider != null)
        {
            if(groundTarget.transform.gameObject.tag == "Target" && groundTarget.distance <= 0.4f)
            {
                m_IsGrounded = true;
                airDecay = 0.0f;
            }
            else
            {
                m_IsGrounded = false;
            }
        }
        else
        {
            m_IsGrounded = false;
        }*/
        #endregion

        #region SphereCast
        //Found a reference to spherecasts while looking for an alternative to casting multiple rays to see if the area under me is clear
        //I implemented this straight from interpretation of documentation

        if (Physics.SphereCast(transform.position + new Vector3(0.0f, 0.0f, 0.0f), 0.48f, Vector3.down, out groundTarget, 0.63f, playerMask) && groundTarget.collider != null)
        {
            //Debug.Log("Colliding");
            if (groundTarget.transform.gameObject.tag == "Target")
            {
                m_IsGrounded = true;
                airDecay = 0.0f;
                //Debug.Log("Ground detected");
            }
            else
            {
                m_IsGrounded = false;
            }
        }
        else
        {
            m_IsGrounded = false;
            //Debug.Log("Airborne");
        }
        #endregion
    }

    private void OnDrawGizmos()
    {
        //Debug sphere for the spherecast destination
        //Used to ensure it was pretty flush against the capsule collider
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + new Vector3(0.0f, -0.63f, 0.0f), 0.48f);
    }

    //Pertaining to the player entering heat fields
    #region OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HeatField")
        {
            //Takes remaining heat from a heat field, then sets it to delete itself.
            //This permits it to carry out other functions before self-erasing.
            //Debug.Log("Entered Heat Field");
            CurrentField = other.gameObject.GetComponent<HeatDissipate>();
            heatGain = CurrentField.heatNow;
            //Debug.Log(heatCurrent);
            CurrentField.deleting = true;
            Instantiate(regenerator, transform.position, Quaternion.identity);
        }

        if (other.tag == "HeatConstant")
        {
            //Increases passive regen while in a constant heat field.
            constantGain = 10.0f;
            //Debug.Log("Entered Constant Field");
        }
    }
    #endregion

    //Pertaining to the player leaving heat fields
    #region OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HeatConstant")
        {
            //Resets passive regen while leaving a constant heat field.
            constantGain = 0.0f;
            //Debug.Log("Left Constant Field");
        }
    }
    #endregion

    #region Gaz' landing
    /*private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Collide!");
        // MATHS EXPLANATION
        
        // 'hit' gives us information about the thing our Collider is touching right now
        // The normal tells us which direction we're touching it on
        // The dot product tells us "How much does this 'normal' direction match the 'up' direction"
        // ... i.e. Is the 'normal' pointing 'up'?

        if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
        {
            // If the above conditions are true, then we've landed on something.
            // Set 'Grounded' to true and set our falling speed to 0
            m_IsGrounded = true;
            m_Velocity.y = 0.0f;
        }
    }
    */
    #endregion
}
