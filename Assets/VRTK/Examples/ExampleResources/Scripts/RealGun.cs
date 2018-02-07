namespace VRTK.Examples
{
    using UnityEngine;

    public class RealGun : VRTK_InteractableObject
    {
        public ParticleSystem muzzleFlash;

        public float bulletSpeed = 200f;
        public float bulletLife = 5f;

		public static int bulletShot = 0;
		private int MagazineSize = 30;

		public Transform GunSpawn;

        public GameObject bullet;

        public float timer;
		public Color c1 = Color.red;
		public Color c2 = Color.yellow;
		public GameObject RightHand;
		public GameObject RightHandAvatar;
		public GameObject RightHandControllerAvatar;

		private VRTK_ControllerEvents controllerEvents;
        public AudioSource gunSound;

        private void ToggleCollision(Rigidbody objRB, Collider objCol, bool state)
        {
            objRB.isKinematic = state;
            objCol.isTrigger = state;
        }

        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject)
        {
            base.Grabbed(currentGrabbingObject);

            controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();

			if (OVRInput.GetActiveController() != OVRInput.Controller.Touchpad)
			{
				//Limit hands grabbing when picked up
				if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject.controllerEvents.gameObject) == SDK_BaseController.ControllerHand.Left)
				{
					allowedTouchControllers = AllowedController.LeftOnly;
					allowedUseControllers = AllowedController.LeftOnly;
				}
				else if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject.controllerEvents.gameObject) == SDK_BaseController.ControllerHand.Right)
				{
					allowedTouchControllers = AllowedController.RightOnly;
					allowedUseControllers = AllowedController.RightOnly;
				}
			}
		}

        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject)
        {
            base.Ungrabbed(previousGrabbingObject);

            //Unlimit hands
            allowedTouchControllers = AllowedController.Both;
            allowedUseControllers = AllowedController.Both;

            controllerEvents = null;
        }

        public override void StartUsing(VRTK_InteractUse currentUsingObject)
        {
            base.StartUsing(currentUsingObject);
        }
        
        public override void StopUsing(VRTK_InteractUse previousUsingObject = null)
        {
            base.StopUsing(previousUsingObject);
            
        }

        protected override void Awake()
        {
            base.Awake();
            bullet.SetActive(false);
            timer = Time.time;

			if (OVRInput.GetActiveController() == OVRInput.Controller.Touchpad)
			{
				if (RightHand == null)
					RightHand = GameObject.Find("RightHandAnchor");

				if (RightHandAvatar == null)
					RightHandAvatar = transform.Find("hand_right").gameObject;

				if (RightHandControllerAvatar == null)
					RightHandControllerAvatar = transform.Find("controller_right").gameObject;

				LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
				lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
				lineRenderer.widthMultiplier = 0.2f;
				lineRenderer.positionCount = 2;

				float alpha = 1.0f;
				Gradient gradient = new Gradient();
				gradient.SetKeys(
					new GradientColorKey[] {new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f)},
					new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
					);
				lineRenderer.colorGradient = gradient;
			}

		}

        protected override void Update()
        {
            base.Update();
            if (IsUsing() && timer < Time.time)
            {
                timer = Time.time +0.1f;
                FireBullet();
            }

			if (OVRInput.GetActiveController() == OVRInput.Controller.Touchpad) //Code For GearVR controls
			{
				RightHandAvatar.transform.position = RightHand.transform.position;
				RightHandControllerAvatar.transform.position = RightHand.transform.position;

				LineRenderer lineRenderer = GetComponent<LineRenderer>();
				lineRenderer.SetPosition(0, RightHand.transform.position);
				lineRenderer.SetPosition(1, RightHand.transform.position + Vector3.forward);

				if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
				{
					if (timer < Time.time)
					{
						timer = Time.time + 0.1f;
						FireBullet();
					}
				}

				if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
				{
					GameManager.instance.StartGame();
				}
			}
		}

        private void FireBullet()
        {
			if (bulletShot < MagazineSize)
			{
				GameObject bulletClone = Instantiate(bullet, bullet.transform.position, bullet.transform.rotation) as GameObject;
                gunSound.Play();
				bulletClone.SetActive(true);
				Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
				rb.AddForce(bullet.transform.forward * bulletSpeed);
				Destroy(bulletClone, bulletLife);
				muzzleFlash.Play();
				VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject), 1f, 0.2f, 0.01f);
				bulletShot++;
				GameManager.instance.updateAmmoCount(bulletShot);
				}
        }

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Floor")
			{
				bulletShot = 0;
				gameObject.transform.position = GunSpawn.transform.position;
				gameObject.transform.rotation = gameObject.transform.rotation;
                GameManager.instance.updateAmmoCount(bulletShot);
            }
		}
	}
}