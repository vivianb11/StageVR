using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class rotation : MonoBehaviour
    {
        public float TimePerRevolution;
        public Transform target;
        private bool _canOrbit;
        private float _angle;
        private float _distance;
        public Rigidbody2D rb;
        private Vector2 _previousPos;
        private float _currentSpeed;
        public bool startOnStart;
        public bool Spaceship;
        [HideInInspector] public UnityEvent<float> OnOrbitEnter;
        private float targetRadius;
        [SerializeField] private int orbitSegments = 64;
        public float planetVolume;

        private Vector3[] unitCircle;
        private Vector2 _velocity;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            unitCircle = new Vector3[orbitSegments];
            float pointsCount = unitCircle.Length - 1;

            for (int i = 0; i < unitCircle.Length; i++)
            {
                float rad = Mathf.Deg2Rad * 360f * (i / pointsCount);
                unitCircle[i] = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            }

            if (startOnStart)
                ToggleOrbit();
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            _canOrbit = false;
            OrbitOn();
        }

        bool randombool()
        {
            return Random.Range(0, 2) == 1;
        }

        void FixedUpdate()
        {
            _velocity = (rb.position - _previousPos) / Time.fixedDeltaTime;
            _previousPos = rb.position;

            UpdateOrbit();
        }

        public void ToggleOrbit(bool keepVelocity = true)
        {
            _canOrbit = !_canOrbit;

            if (_canOrbit)
            {
                OrbitOn();
            }
            else
            {
                OrbitOff();
            }
        }

        public void OrbitOn()
        {
            if (!target)
            {
                _canOrbit = false;
                return;
            }

            _canOrbit = true;
            _currentSpeed = rb.velocity.magnitude;
            rb.velocity = Vector3.zero;
            Vector3 fromTarget = transform.position - target.position;
            _angle = Vector3.SignedAngle(Vector3.right, fromTarget, Vector3.forward);
            _distance = fromTarget.magnitude;
            _previousPos = rb.position;
            targetRadius = Mathf.Max(target.lossyScale.x, target.lossyScale.y, target.lossyScale.z) / 2;
            OnOrbitEnter.Invoke(_distance);


            if (Spaceship)
            {
                Vector3 orbitDirection = Quaternion.AngleAxis(90f, Vector3.forward) * fromTarget;

                float sign = Mathf.Sign(Vector3.Dot(_velocity.normalized, orbitDirection));
                TimePerRevolution = sign * Mathf.Abs(TimePerRevolution);
            }
        }

        public void OrbitOff()
        {
            _canOrbit = false;
            rb.velocity = _velocity;
            targetRadius = 0;
            _distance = 0;

        }

        public void SetRevolutionTime(float revolution)
        {
            TimePerRevolution = Mathf.Sign(TimePerRevolution) * revolution;
        }

        private void ShowOrbit()
        {
            Vector3[] positions = new Vector3[unitCircle.Length];
            Vector3 pos = target.position;

            for (int i = 0; i < unitCircle.Length; i++)
            {
                positions[i] = pos + unitCircle[i] * _distance;
            }
        }

        void UpdateOrbit()
        {
            // Break the orbit if no target
            _canOrbit = _canOrbit && target;

            if (!_canOrbit)
                return;

            ShowOrbit();

            _currentSpeed = Mathf.Lerp(_currentSpeed, 360f / TimePerRevolution, Time.fixedDeltaTime);
            _angle += _currentSpeed * targetRadius / _distance * Time.fixedDeltaTime;

            Quaternion q = Quaternion.AngleAxis(_angle, Vector3.forward);
            rb.MovePosition(target.position + (q * Vector3.right * _distance));

            if (Spaceship)
            {
                Quaternion qp = TimePerRevolution >= 0 ? q : q * Quaternion.AngleAxis(180f, Vector3.forward);
                rb.MoveRotation(Quaternion.Lerp(Quaternion.Euler(0f, 0f, rb.rotation), qp, Time.fixedDeltaTime));
            }
        }

    }
}