using System.Collections;
using System.Collections.Generic;
using System.IO;

// 尚未使用 
// 缺少文档

// Ilmenite Object Notation
public abstract class ION
{
    public static string[] CST_TAGS = new string[]
    {
        "NULL",
        "BYTE",
        "SHORT",
        "INT",
        "LONG",
        "FLOAT",
        "DOUBLE",
        "BYTE[]",
        "STRING",
        "LIST",
        "DICT",
    };

    abstract internal byte GetTagID();

    abstract public long HashCode();

    abstract public ION Copy();

    abstract public int AsInt();
    abstract public long AsLong();
    abstract public float AsFloat();
    abstract public double AsDouble();
    abstract public byte[] AsByteArray();
    abstract public string AsString();
    abstract public List<ION> AsList();
    abstract public Dictionary<string, ION> AsDict();

    public static implicit operator ION(byte data)
    {
        return new IONTagByte(data);
    }
    public static implicit operator ION(short data)
    {
        return new IONTagShort(data);
    }
    public static implicit operator ION(int data)
    {
        return new IONTagInt(data);
    }
    public static implicit operator ION(long data)
    {
        return new IONTagLong(data);
    }
    public static implicit operator ION(float data)
    {
        return new IONTagFloat(data);
    }
    public static implicit operator ION(double data)
    {
        return new IONTagDouble(data);
    }
    public static implicit operator ION(byte[] data)
    {
        return new IONTagByteArray(data);
    }
    public static implicit operator ION(string data)
    {
        return new IONTagString(data);
    }
    public static implicit operator ION(List<ION> data)
    {
        return new IONTagList(data);
    }
    public static implicit operator ION(Dictionary<string, ION> data)
    {
        return new IONTagDict(data);
    }

    override abstract public string ToString();
    abstract public string ToJson();

    abstract internal void WriteContent(Stream stream);
    abstract internal void ReadContent(Stream stream);

    public static ION Read(Stream stream)
    {
        byte[] buffer = new byte[1];
        stream.Read(buffer, 0, 1);
        byte id = buffer[0];
        ION ret;
        switch (id)
        {
            case 0:
                ret = new IONTagNULL();
                break;
            case 1:
                ret = new IONTagByte();
                break;
            case 2:
                ret = new IONTagShort();
                break;
            case 3:
                ret = new IONTagInt();
                break;
            case 4:
                ret = new IONTagLong();
                break;
            case 5:
                ret = new IONTagFloat();
                break;
            case 6:
                ret = new IONTagDouble();
                break;
            case 7:
                ret = new IONTagByteArray();
                break;
            case 8:
                ret = new IONTagString();
                break;
            case 9:
                ret = new IONTagList();
                break;
            case 10:
                ret = new IONTagDict();
                break;
            default:
                throw new System.Exception("Loading ION file error!");
        }
        ret.ReadContent(stream);
        return ret;
    }
    public void Write(Stream stream)
    {
        stream.Write(new byte[] { this.GetTagID() }, 0, 1);
        this.WriteContent(stream);
    }
}

internal class IONTagNULL: ION
{
    internal override byte GetTagID()
    {
        return 0;
    }

    override public long HashCode()
    {
        return 0;
    }

    override public ION Copy()
    {
        return new IONTagNULL();
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }
    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return "null";
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
    }

    internal override void ReadContent(Stream stream)
    {
    }
}

internal class IONTagByte : ION
{
    internal override byte GetTagID()
    {
        return 1;
    }
    byte data;

    internal IONTagByte(byte data)
    {
        this.data = data;
    }

    public IONTagByte()
    {
    }

    override public long HashCode()
    {
        return data;
    }

    override public ION Copy()
    {
        return new IONTagByte(data);
    }

    public override int AsInt()
    {
        return data;
    }

    public override long AsLong()
    {
        return data;
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return data.ToString();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes(data), 0, 1);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[1];
        stream.Read(buffer, 0, 1);
        data = buffer[0];
    }
}

internal class IONTagShort : ION
{
    internal override byte GetTagID()
    {
        return 2;
    }
    short data;

    internal IONTagShort(short data)
    {
        this.data = data;
    }

    public IONTagShort()
    {
    }

    override public long HashCode()
    {
        return data;
    }

    override public ION Copy()
    {
        return new IONTagShort(data);
    }

    public override int AsInt()
    {
        return data;
    }

    public override long AsLong()
    {
        return data;
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return data.ToString();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes(data), 0, 2);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[2];
        stream.Read(buffer, 0, 2);
        data = System.BitConverter.ToInt16(buffer, 0);
    }

}

internal class IONTagInt : ION
{
    internal override byte GetTagID()
    {
        return 3;
    }
    int data;

    internal IONTagInt(int data)
    {
        this.data = data;
    }

    public IONTagInt()
    {
    }

    override public long HashCode()
    {
        return data;
    }

    override public ION Copy()
    {
        return new IONTagInt(data);
    }

    public override int AsInt()
    {
        return data;
    }

    public override long AsLong()
    {
        return data;
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return data.ToString();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes(data), 0, 4);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        data = System.BitConverter.ToInt32(buffer, 0);
    }

}

internal class IONTagLong : ION
{
    internal override byte GetTagID()
    {
        return 4;
    }
    long data;

    internal IONTagLong(long data)
    {
        this.data = data;
    }

    public IONTagLong()
    {
    }

    override public long HashCode()
    {
        return data;
    }

    override public ION Copy()
    {
        return new IONTagLong(data);
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        return data;
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return data.ToString();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes(data), 0, 8);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[8];
        stream.Read(buffer, 0, 8);
        data = System.BitConverter.ToInt64(buffer, 0);
    }

}

internal class IONTagFloat : ION
{
    internal override byte GetTagID()
    {
        return 5;
    }
    float data;

    internal IONTagFloat(float data)
    {
        this.data = data;
    }

    public IONTagFloat()
    {
    }

    override public long HashCode()
    {
        return (long)data;
    }

    override public ION Copy()
    {
        return new IONTagFloat(data);
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        return data;
    }

    public override double AsDouble()
    {
        return data;
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return data.ToString();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes(data), 0, 4);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        data = System.BitConverter.ToSingle(buffer, 0);
    }

}

internal class IONTagDouble : ION
{
    internal override byte GetTagID()
    {
        return 6;
    }
    double data;

    internal IONTagDouble(double data)
    {
        this.data = data;
    }

    public IONTagDouble()
    {
    }

    override public long HashCode()
    {
        return (long)data;
    }

    override public ION Copy()
    {
        return new IONTagDouble(data);
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        return (float)data;
    }

    public override double AsDouble()
    {
        return data;
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return data.ToString();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes(data), 0, 8);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[8];
        stream.Read(buffer, 0, 8);
        data = System.BitConverter.ToDouble(buffer, 0);
    }
}

internal class IONTagByteArray : ION
{
    internal override byte GetTagID()
    {
        return 7;
    }
    byte[] data;

    internal IONTagByteArray(byte[] data)
    {
        this.data = data;
    }

    public IONTagByteArray()
    {
    }

    override public long HashCode()
    {
        long ret = 0;
        for (int i = 0; i < data.Length; ++ i)
        {
            ret = ret * 13 + data[i];
        }
        return ret;
    }

    override public ION Copy()
    {
        return new IONTagByteArray(data);
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        return data;
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    // TODO
    public override string ToString()
    {
        throw new System.NotImplementedException();
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes((int)data.Length), 0, 4);
        stream.Write(data, 0, data.Length);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        int len = System.BitConverter.ToInt32(buffer, 0);
        data = new byte[len];
        stream.Read(data, 0, len);
    }
}

internal class IONTagString : ION
{
    internal override byte GetTagID()
    {
        return 8;
    }
    string data;

    internal IONTagString(string data)
    {
        this.data = data;
    }

    public IONTagString()
    {
    }

    override public long HashCode()
    {
        long ret = 0;
        for (int i = 0; i < data.Length; ++i)
        {
            ret = ret * 17 + data[i];
        }
        return ret;
    }

    override public ION Copy()
    {
        return new IONTagString(data);
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        return data;
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        return "\"" + data + "\"";
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);
        stream.Write(System.BitConverter.GetBytes((int)buffer.Length), 0, 4);
        stream.Write(buffer, 0, buffer.Length);
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        int len = System.BitConverter.ToInt32(buffer, 0);
        buffer = new byte[len];
        stream.Read(buffer, 0, len);
        data = System.Text.Encoding.UTF8.GetString(buffer);
    }
}

internal class IONTagList : ION
{
    internal override byte GetTagID()
    {
        return 9;
    }
    List<ION> data;

    internal IONTagList(List<ION> data)
    {
        this.data = data;
    }

    public IONTagList()
    {
    }

    override public long HashCode()
    {
        long ret = 0;
        foreach (ION item in data)
        {
            ret = ret * 23 + item.HashCode();
        }
        return ret;
    }

    override public ION Copy()
    {
        throw new System.NotImplementedException();
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        return data;
    }

    public override Dictionary<string, ION> AsDict()
    {
        throw new System.NotImplementedException();
    }

    public override string ToString()
    {
        string ret = null;
        foreach (ION item in data)
        {
            if (ret == null)
            {
                ret = item.ToString();
            } else
            {
                ret += ", " + item.ToString();
            }
        }
        return "[" + ret + ']';
    }

    public override string ToJson()
    {
        throw new System.NotImplementedException();
    }

    internal override void WriteContent(Stream stream)
    {
        stream.Write(System.BitConverter.GetBytes((int)data.Count), 0, 4);
        foreach (ION item in data)
        {
            item.Write(stream);
        }
    }

    internal override void ReadContent(Stream stream)
    {
        byte[] buffer = new byte[4];
        stream.Read(buffer, 0, 4);
        int count = System.BitConverter.ToInt32(buffer, 0);
        data = new List<ION>();
        for (int i = 0; i < count; ++ i)
        {
            data.Add(ION.Read(stream));
        }
    }
}

internal class IONTagDict: ION
{
    internal override byte GetTagID()
    {
        return 10;
    }
    Dictionary<string, ION> data;

    internal IONTagDict(Dictionary<string, ION> data)
    {
        this.data = data;
    }

    public IONTagDict()
    {
    }

    // TODO
    override public long HashCode()
    {
        return 65565;
    }

    override public ION Copy()
    {
        throw new System.NotImplementedException();
    }

    public override int AsInt()
    {
        throw new System.NotImplementedException();
    }

    public override long AsLong()
    {
        throw new System.NotImplementedException();
    }

    public override float AsFloat()
    {
        throw new System.NotImplementedException();
    }

    public override double AsDouble()
    {
        throw new System.NotImplementedException();
    }

    public override byte[] AsByteArray()
    {
        throw new System.NotImplementedException();
    }

    public override string AsString()
    {
        throw new System.NotImplementedException();
    }

    public override List<ION> AsList()
    {
        throw new System.NotImplementedException();
    }

    public override Dictionary<string, ION> AsDict()
    {
        return data;
    }

    public override string ToString()
    {
        string ret = null;
        foreach (string item in data.Keys)
        {
            if (ret == null)
            {
                ret = "\"" + item + "\":" + data[item].ToString();
            } else
            {
                ret += ",\"" + item + "\":" + data[item].ToString();
            }
        }
        return "{" + ret + "}";
    }

    public override string ToJson()
    {
        return this.ToString();
    }

    internal override void WriteContent(Stream stream)
    {
        foreach (string key in data.Keys)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(key);
            // TODO 异常判断
            stream.Write(System.BitConverter.GetBytes((short)buffer.Length), 0, 2);
            stream.Write(buffer, 0, buffer.Length);
            data[key].Write(stream);
        }
        stream.Write(new byte[]{ 0, 0 }, 0, 2);
    }

    internal override void ReadContent(Stream stream)
    {
        data = new Dictionary<string, ION>();
        while (true)
        {
            byte[] buffer = new byte[2];
            stream.Read(buffer, 0, 2);
            short count = System.BitConverter.ToInt16(buffer, 0);
            if (count == 0)
                break;
            buffer = new byte[count];
            stream.Read(buffer, 0, count);
            string key = System.Text.Encoding.UTF8.GetString(buffer);
            ION value = ION.Read(stream);
            data.Add(key, value);
        }
    }
}