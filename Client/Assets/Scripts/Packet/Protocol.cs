// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protocol.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Protobuf.Protocol {

  /// <summary>Holder for reflection information generated from Protocol.proto</summary>
  public static partial class ProtocolReflection {

    #region Descriptor
    /// <summary>File descriptor for Protocol.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtocolReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg5Qcm90b2NvbC5wcm90bxIIUHJvdG9jb2waH2dvb2dsZS9wcm90b2J1Zi90",
            "aW1lc3RhbXAucHJvdG8iCQoHQ19FbnRlciIlChBTX0VudGVyX1Jlc3BvbnNl",
            "EhEKCXNlc3Npb25JRBgBIAEoBSIpCgZDX01vdmUSHwoDcG9zGAEgASgLMhIu",
            "UHJvdG9jb2wuUG9zaXRpb24iRgoQU19Ccm9hZGNhc3RfTW92ZRIRCglzZXNz",
            "aW9uSUQYASABKAUSHwoDcG9zGAIgASgLMhIuUHJvdG9jb2wuUG9zaXRpb24i",
            "KwoIUG9zaXRpb24SCQoBeBgBIAEoAhIJCgF5GAIgASgCEgkKAXoYAyABKAIq",
            "TAoFTXNnSUQSCwoHQ19FTlRFUhAAEhQKEFNfRU5URVJfUkVTUE9OU0UQARIK",
            "CgZDX01PVkUQAhIUChBTX0JST0FEQ0FTVF9NT1ZFEANCG6oCGEdvb2dsZS5Q",
            "cm90b2J1Zi5Qcm90b2NvbGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Google.Protobuf.Protocol.MsgID), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Protocol.C_Enter), global::Google.Protobuf.Protocol.C_Enter.Parser, null, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Protocol.S_Enter_Response), global::Google.Protobuf.Protocol.S_Enter_Response.Parser, new[]{ "SessionID" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Protocol.C_Move), global::Google.Protobuf.Protocol.C_Move.Parser, new[]{ "Pos" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Protocol.S_Broadcast_Move), global::Google.Protobuf.Protocol.S_Broadcast_Move.Parser, new[]{ "SessionID", "Pos" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Protocol.Position), global::Google.Protobuf.Protocol.Position.Parser, new[]{ "X", "Y", "Z" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum MsgID {
    /// <summary>
    ///option allow_alias = true;	//Other Enums can get same numerics
    /// </summary>
    [pbr::OriginalName("C_ENTER")] CEnter = 0,
    [pbr::OriginalName("S_ENTER_RESPONSE")] SEnterResponse = 1,
    [pbr::OriginalName("C_MOVE")] CMove = 2,
    [pbr::OriginalName("S_BROADCAST_MOVE")] SBroadcastMove = 3,
  }

  #endregion

  #region Messages
  public sealed partial class C_Enter : pb::IMessage<C_Enter> {
    private static readonly pb::MessageParser<C_Enter> _parser = new pb::MessageParser<C_Enter>(() => new C_Enter());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<C_Enter> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Protocol.ProtocolReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C_Enter() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C_Enter(C_Enter other) : this() {
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C_Enter Clone() {
      return new C_Enter(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as C_Enter);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(C_Enter other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(C_Enter other) {
      if (other == null) {
        return;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
        }
      }
    }

  }

  public sealed partial class S_Enter_Response : pb::IMessage<S_Enter_Response> {
    private static readonly pb::MessageParser<S_Enter_Response> _parser = new pb::MessageParser<S_Enter_Response>(() => new S_Enter_Response());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<S_Enter_Response> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Protocol.ProtocolReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S_Enter_Response() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S_Enter_Response(S_Enter_Response other) : this() {
      sessionID_ = other.sessionID_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S_Enter_Response Clone() {
      return new S_Enter_Response(this);
    }

    /// <summary>Field number for the "sessionID" field.</summary>
    public const int SessionIDFieldNumber = 1;
    private int sessionID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SessionID {
      get { return sessionID_; }
      set {
        sessionID_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as S_Enter_Response);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(S_Enter_Response other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SessionID != other.SessionID) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (SessionID != 0) hash ^= SessionID.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (SessionID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(SessionID);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (SessionID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SessionID);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(S_Enter_Response other) {
      if (other == null) {
        return;
      }
      if (other.SessionID != 0) {
        SessionID = other.SessionID;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            SessionID = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class C_Move : pb::IMessage<C_Move> {
    private static readonly pb::MessageParser<C_Move> _parser = new pb::MessageParser<C_Move>(() => new C_Move());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<C_Move> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Protocol.ProtocolReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C_Move() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C_Move(C_Move other) : this() {
      pos_ = other.pos_ != null ? other.pos_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public C_Move Clone() {
      return new C_Move(this);
    }

    /// <summary>Field number for the "pos" field.</summary>
    public const int PosFieldNumber = 1;
    private global::Google.Protobuf.Protocol.Position pos_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.Protocol.Position Pos {
      get { return pos_; }
      set {
        pos_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as C_Move);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(C_Move other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Pos, other.Pos)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (pos_ != null) hash ^= Pos.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (pos_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Pos);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (pos_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Pos);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(C_Move other) {
      if (other == null) {
        return;
      }
      if (other.pos_ != null) {
        if (pos_ == null) {
          Pos = new global::Google.Protobuf.Protocol.Position();
        }
        Pos.MergeFrom(other.Pos);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (pos_ == null) {
              Pos = new global::Google.Protobuf.Protocol.Position();
            }
            input.ReadMessage(Pos);
            break;
          }
        }
      }
    }

  }

  public sealed partial class S_Broadcast_Move : pb::IMessage<S_Broadcast_Move> {
    private static readonly pb::MessageParser<S_Broadcast_Move> _parser = new pb::MessageParser<S_Broadcast_Move>(() => new S_Broadcast_Move());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<S_Broadcast_Move> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Protocol.ProtocolReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S_Broadcast_Move() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S_Broadcast_Move(S_Broadcast_Move other) : this() {
      sessionID_ = other.sessionID_;
      pos_ = other.pos_ != null ? other.pos_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public S_Broadcast_Move Clone() {
      return new S_Broadcast_Move(this);
    }

    /// <summary>Field number for the "sessionID" field.</summary>
    public const int SessionIDFieldNumber = 1;
    private int sessionID_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int SessionID {
      get { return sessionID_; }
      set {
        sessionID_ = value;
      }
    }

    /// <summary>Field number for the "pos" field.</summary>
    public const int PosFieldNumber = 2;
    private global::Google.Protobuf.Protocol.Position pos_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.Protocol.Position Pos {
      get { return pos_; }
      set {
        pos_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as S_Broadcast_Move);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(S_Broadcast_Move other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SessionID != other.SessionID) return false;
      if (!object.Equals(Pos, other.Pos)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (SessionID != 0) hash ^= SessionID.GetHashCode();
      if (pos_ != null) hash ^= Pos.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (SessionID != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(SessionID);
      }
      if (pos_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Pos);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (SessionID != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(SessionID);
      }
      if (pos_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Pos);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(S_Broadcast_Move other) {
      if (other == null) {
        return;
      }
      if (other.SessionID != 0) {
        SessionID = other.SessionID;
      }
      if (other.pos_ != null) {
        if (pos_ == null) {
          Pos = new global::Google.Protobuf.Protocol.Position();
        }
        Pos.MergeFrom(other.Pos);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            SessionID = input.ReadInt32();
            break;
          }
          case 18: {
            if (pos_ == null) {
              Pos = new global::Google.Protobuf.Protocol.Position();
            }
            input.ReadMessage(Pos);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Position : pb::IMessage<Position> {
    private static readonly pb::MessageParser<Position> _parser = new pb::MessageParser<Position>(() => new Position());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Position> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Protocol.ProtocolReflection.Descriptor.MessageTypes[4]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Position() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Position(Position other) : this() {
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Position Clone() {
      return new Position(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private float x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private float y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 3;
    private float z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Position);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Position other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(X, other.X)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Y, other.Y)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Z, other.Z)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(X);
      if (Y != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Y);
      if (Z != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Z);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(Y);
      }
      if (Z != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Z);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0F) {
        size += 1 + 4;
      }
      if (Y != 0F) {
        size += 1 + 4;
      }
      if (Z != 0F) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Position other) {
      if (other == null) {
        return;
      }
      if (other.X != 0F) {
        X = other.X;
      }
      if (other.Y != 0F) {
        Y = other.Y;
      }
      if (other.Z != 0F) {
        Z = other.Z;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 13: {
            X = input.ReadFloat();
            break;
          }
          case 21: {
            Y = input.ReadFloat();
            break;
          }
          case 29: {
            Z = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
