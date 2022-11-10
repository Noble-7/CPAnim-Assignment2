using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpline : MonoBehaviour
{
	public Transform[] points;
	public float speed = 1f;
	[Range(1, 32)]
	public int sampleRate = 16;

	[System.Serializable]
	class SamplePoint
	{
		public float samplePosition;
		public float accumulatedDistance;

		public SamplePoint(float samplePosition, float distanceCovered)
		{
			this.samplePosition = samplePosition;
			this.accumulatedDistance = distanceCovered;
		}
	}
	//list of segment samples makes it easier to index later
	//imagine it like List<SegmentSamples>, and segment sample is a list of SamplePoints
	List<List<SamplePoint>> table = new List<List<SamplePoint>>();

	float distance = 0f;
	float accumDistance = 0f;
	int currentIndex = 0;
	int currentSample = 0;

	private void Start()
	{
		//make sure there are 4 points, else disable the component
		if (points.Length < 4)
		{
			enabled = false;
		}

		int size = points.Length;

		//calculate the speed graph table
		Vector3 prevPos = points[0].position;
		for (int i = 0; i < size; ++i)
		{
			List<SamplePoint> segment = new List<SamplePoint>();
			Vector3 p0 = points[(i - 1 + points.Length) % points.Length].position;
			Vector3 p1 = points[i].position;
			Vector3 p2 = points[(i + 1) % points.Length].position;
			Vector3 p3 = points[(i + 2) % points.Length].position;

			//calculate samples
			segment.Add(new SamplePoint(0f, accumDistance));
			Vector3 lastPos = GetCatmullRomPosition(p0, p1, p2, p3, 0);
			for (int sample = 1; sample <= sampleRate; ++sample)
			{
				//TODO: create each sample and store in segment
				float t = (float)sample / sampleRate;
				Vector3 newPos = GetCatmullRomPosition(p0, p1, p2, p3, t);
				Vector3 result = newPos - lastPos;
				accumDistance += result.magnitude;
				segment.Add(new SamplePoint(t, accumDistance));
				lastPos = newPos;
			}
			table.Add(segment);
		}
	}

	private void Update()
	{
		distance += speed * Time.deltaTime;

		//check if we need to update our samples
		while (distance > table[currentIndex][currentSample + 1].accumulatedDistance)
		{
			//TODO: update sample and index indices
			if (currentSample >= sampleRate - 1)
			{
				if (currentIndex < table.Count - 1)
				{
					currentIndex++;
				}
				else
				{
					currentIndex = 0;
					distance = 0;
				}
				currentSample = 0;
			}
			else
			{
				currentSample++;
			}
		}

		Vector3 p0 = points[(currentIndex - 1 + points.Length) % points.Length].position;
		Vector3 p1 = points[currentIndex].position;
		Vector3 p2 = points[(currentIndex + 1) % points.Length].position;
		Vector3 p3 = points[(currentIndex + 2) % points.Length].position;

		transform.position = GetCatmullRomPosition(p0, p1, p2, p3, GetAdjustedT());
	}

	Vector3 GetCatmullRomPosition(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}

	float GetAdjustedT()
	{
		SamplePoint current = table[currentIndex][currentSample];
		SamplePoint next = table[currentIndex][currentSample + 1];

		return Mathf.Lerp(current.samplePosition, next.samplePosition,
			(distance - current.accumulatedDistance) / (next.accumulatedDistance - current.accumulatedDistance)
		);
	}


	private void OnDrawGizmos()
	{
		Vector3 a, b, p0, p1, p2, p3;
		for (int i = 0; i < points.Length; i++)
		{
			a = points[i].position;
			p0 = points[(points.Length + i - 1) % points.Length].position;
			p1 = points[i].position;
			p2 = points[(i + 1) % points.Length].position;
			p3 = points[(i + 2) % points.Length].position;
			for (int j = 1; j <= sampleRate; ++j)
			{
				b = GetCatmullRomPosition(p0, p1, p2, p3, (float)j / sampleRate);
				Gizmos.DrawLine(a, b);
				a = b;
			}
		}
	}
}
