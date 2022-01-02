using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaManager
{
	public struct Point
	{
		public double x;
		public double y;
		public double z;
	}

	public class MANAGER
	{

		private ArenaNET.ISystem m_system = null;
		private List<ArenaNET.IDeviceInfo> m_deviceInfos;
		private ArenaNET.IDevice m_connectedDevice = null;
		private ArenaNET.IImage m_converted;

		ArenaNET.IImage image = null;

		public MANAGER()
		{
			m_system = ArenaNET.Arena.OpenSystem();
			Console.WriteLine("생성자 생성 표시");
		}

		~MANAGER()
		{
			Console.WriteLine("프로그램 소멸 표시");
		}

		private void UpdateDevices()
		{
			m_system.UpdateDevices(100);
			m_deviceInfos = m_system.Devices;
		}
		public void ConnectDevice()
		{
			UpdateDevices();

			Console.WriteLine("device: {0}", (m_deviceInfos.Count));

			if (m_deviceInfos.Count == 0)
				{
					Console.WriteLine("\nNo camera connected\nPress enter to complete");
					Console.Read();
				}

				else
				{
					m_connectedDevice = m_system.CreateDevice(m_deviceInfos[0]);
				}
		}

		public void StartStream()
		{
			if (m_connectedDevice != null)
			{
				if ((m_connectedDevice.TLStreamNodeMap.GetNode("StreamIsGrabbing") as ArenaNET.IBoolean).Value == false)
				{
					(m_connectedDevice.TLStreamNodeMap.GetNode("StreamBufferHandlingMode") as ArenaNET.IEnumeration).Symbolic = "NewestOnly";
					m_connectedDevice.StartStream();
				}
			}
		}

		public void GetIImage(UInt32 timeout = 2000)
		{
			if (m_converted != null)
			{
				ArenaNET.ImageFactory.Destroy(m_converted);
				m_converted = null;
			}

			if (image != null)
			{
				m_connectedDevice.RequeueBuffer(image);
				image = null;
			}

			image = m_connectedDevice.GetImage(timeout);
		}

		public Point[] byteToFloatArray()
		{

			var Scan3dCoordinateSelectorNode = (ArenaNET.IEnumeration)m_connectedDevice.NodeMap.GetNode("Scan3dCoordinateSelector");
			var Scan3dCoordinateScaleNode = (ArenaNET.IFloat)m_connectedDevice.NodeMap.GetNode("Scan3dCoordinateScale");
			var Scan3dCoordinateOffsetNode = (ArenaNET.IFloat)m_connectedDevice.NodeMap.GetNode("Scan3dCoordinateOffset");

			Scan3dCoordinateSelectorNode.FromString("CoordinateA");
			float scaleX = (float)Scan3dCoordinateScaleNode.Value;
			float offsetX = (float)Scan3dCoordinateOffsetNode.Value;

			Scan3dCoordinateSelectorNode.FromString("CoordinateB");
			double scaleY = Scan3dCoordinateScaleNode.Value;
			float offsetY = (float)Scan3dCoordinateOffsetNode.Value;

			Scan3dCoordinateSelectorNode.FromString("CoordinateC");
			double scaleZ = Scan3dCoordinateScaleNode.Value;

			// 생성 1. 입력 버퍼로부터 이미지 정보 받아오기
			UInt32 width = image.Width;
			UInt32 height = image.Height;
			UInt32 size = width * height;
			UInt32 srcBpp = image.BitsPerPixel;
			UInt32 srcPixelSize = srcBpp / 8;
			byte[] data = image.DataArray;

			int index = 0;
			Point[] converted = new Point[size]; // size * (x, y, z)

			// 생성 2. byte[] 이미지를 float[] 배열로 변환하기
			for (int i = 0; i < size; i++)
			{
				ushort x = BitConverter.ToUInt16(data, index);
				ushort y = BitConverter.ToUInt16(data, index + 2);
				ushort z = BitConverter.ToUInt16(data, index + 4);

				converted[i].x = (x * scaleX + offsetX);
				converted[i].y = (y * scaleY + offsetY);
				converted[i].z = (z * scaleZ);

				index += (int)srcPixelSize;
			}

			return converted;
		}

		public Point[] YZFiltering(Point[] data)
		{
			// 1. y 필터링
			int start_index = 230 * 640;
			int end_index = 380 * 640;
			int length_index = end_index - start_index;

			Point[] cutted = data.Skip(start_index).Take(length_index).ToArray();
			Point[] cutted2 = new Point[cutted.Length];
			int j = 0;

			// 2. z 필터링
			for(int i = 0; i < cutted.Length; i++)
			{
				if(cutted[i].z < 820 && cutted[i].z > 600)
				{
					//Console.Write("x, y, z: {0} {1} {2}\n", cutted[i].x, cutted[i].y, cutted[i].z);
					cutted2[j].x = cutted[i].x;
					cutted2[j].y = cutted[i].y;
					cutted2[j].z = cutted[i].z;
					j++;
				}
			}

			Console.Write("Vertex_Count: {0}\n", j);

			return cutted2;
		}

        public Point[] MakeTree()
        {
            Point[] tree = new Point[100];

            for(int i = 0; i < 100; i++)
            {
                tree[i].z = i/99;
                tree[i].x = Math.Sin(tree[i].z);
                tree[i].y = Math.Cos(tree[i].z);
            }

            return tree;
        }

        public Point[] GPCA_Filtering(Point[] data)
        {
            return data;
        }



        public void StopStream()
		{
			if (m_connectedDevice != null)
			{
				if ((m_connectedDevice.TLStreamNodeMap.GetNode("StreamIsGrabbing") as ArenaNET.IBoolean).Value == true)
				{
					m_connectedDevice.StopStream();
				}
			}
		}

		public void DisconnectDevice()
		{
			m_system.DestroyDevice(m_connectedDevice);
			m_connectedDevice = null;
		}

		public void CloseSystem()
		{
			ArenaNET.Arena.CloseSystem(m_system);
		}
	}

}
