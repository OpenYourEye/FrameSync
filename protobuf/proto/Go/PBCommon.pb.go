// Code generated by protoc-gen-go. DO NOT EDIT.
// source: PBCommon.proto

/*
Package PBCommon is a generated protocol buffer package.

It is generated from these files:
	PBCommon.proto

It has these top-level messages:
*/
package PBCommon

import proto "github.com/golang/protobuf/proto"
import fmt "fmt"
import math "math"

// Reference imports to suppress errors if they are not otherwise used.
var _ = proto.Marshal
var _ = fmt.Errorf
var _ = math.Inf

// This is a compile-time assertion to ensure that this generated file
// is compatible with the proto package it is being compiled against.
// A compilation error at this line likely means your copy of the
// proto package needs to be updated.
const _ = proto.ProtoPackageIsVersion2 // please upgrade the proto package

type CSID int32

const (
	CSID_TCP_LOGIN                CSID = 1
	CSID_TCP_REQUEST_MATCH        CSID = 10
	CSID_TCP_CANCEL_MATCH         CSID = 11
	CSID_UDP_BATTLE_READY         CSID = 51
	CSID_UDP_UP_PLAYER_OPERATIONS CSID = 53
	CSID_UDP_UP_DELTA_FRAMES      CSID = 55
	CSID_UDP_UP_GAME_OVER         CSID = 57
)

var CSID_name = map[int32]string{
	1:  "TCP_LOGIN",
	10: "TCP_REQUEST_MATCH",
	11: "TCP_CANCEL_MATCH",
	51: "UDP_BATTLE_READY",
	53: "UDP_UP_PLAYER_OPERATIONS",
	55: "UDP_UP_DELTA_FRAMES",
	57: "UDP_UP_GAME_OVER",
}
var CSID_value = map[string]int32{
	"TCP_LOGIN":                1,
	"TCP_REQUEST_MATCH":        10,
	"TCP_CANCEL_MATCH":         11,
	"UDP_BATTLE_READY":         51,
	"UDP_UP_PLAYER_OPERATIONS": 53,
	"UDP_UP_DELTA_FRAMES":      55,
	"UDP_UP_GAME_OVER":         57,
}

func (x CSID) Enum() *CSID {
	p := new(CSID)
	*p = x
	return p
}
func (x CSID) String() string {
	return proto.EnumName(CSID_name, int32(x))
}
func (x *CSID) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(CSID_value, data, "CSID")
	if err != nil {
		return err
	}
	*x = CSID(value)
	return nil
}
func (CSID) EnumDescriptor() ([]byte, []int) { return fileDescriptor0, []int{0} }

type SCID int32

const (
	SCID_TCP_RESPONSE_LOGIN         SCID = 1
	SCID_TCP_RESPONSE_REQUEST_MATCH SCID = 10
	SCID_TCP_RESPONSE_CANCEL_MATCH  SCID = 11
	SCID_TCP_ENTER_BATTLE           SCID = 50
	SCID_UDP_BATTLE_START           SCID = 51
	SCID_UDP_DOWN_FRAME_OPERATIONS  SCID = 53
	SCID_UDP_DOWN_DELTA_FRAMES      SCID = 55
	SCID_UDP_DOWN_GAME_OVER         SCID = 57
)

var SCID_name = map[int32]string{
	1:  "TCP_RESPONSE_LOGIN",
	10: "TCP_RESPONSE_REQUEST_MATCH",
	11: "TCP_RESPONSE_CANCEL_MATCH",
	50: "TCP_ENTER_BATTLE",
	51: "UDP_BATTLE_START",
	53: "UDP_DOWN_FRAME_OPERATIONS",
	55: "UDP_DOWN_DELTA_FRAMES",
	57: "UDP_DOWN_GAME_OVER",
}
var SCID_value = map[string]int32{
	"TCP_RESPONSE_LOGIN":         1,
	"TCP_RESPONSE_REQUEST_MATCH": 10,
	"TCP_RESPONSE_CANCEL_MATCH":  11,
	"TCP_ENTER_BATTLE":           50,
	"UDP_BATTLE_START":           51,
	"UDP_DOWN_FRAME_OPERATIONS":  53,
	"UDP_DOWN_DELTA_FRAMES":      55,
	"UDP_DOWN_GAME_OVER":         57,
}

func (x SCID) Enum() *SCID {
	p := new(SCID)
	*p = x
	return p
}
func (x SCID) String() string {
	return proto.EnumName(SCID_name, int32(x))
}
func (x *SCID) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(SCID_value, data, "SCID")
	if err != nil {
		return err
	}
	*x = SCID(value)
	return nil
}
func (SCID) EnumDescriptor() ([]byte, []int) { return fileDescriptor0, []int{1} }

type TeamType int32

const (
	TeamType_NO    TeamType = 0
	TeamType_TEAM1 TeamType = 1
	TeamType_TEAM2 TeamType = 2
	TeamType_TEAM3 TeamType = 3
	TeamType_TEAM4 TeamType = 4
)

var TeamType_name = map[int32]string{
	0: "NO",
	1: "TEAM1",
	2: "TEAM2",
	3: "TEAM3",
	4: "TEAM4",
}
var TeamType_value = map[string]int32{
	"NO":    0,
	"TEAM1": 1,
	"TEAM2": 2,
	"TEAM3": 3,
	"TEAM4": 4,
}

func (x TeamType) Enum() *TeamType {
	p := new(TeamType)
	*p = x
	return p
}
func (x TeamType) String() string {
	return proto.EnumName(TeamType_name, int32(x))
}
func (x *TeamType) UnmarshalJSON(data []byte) error {
	value, err := proto.UnmarshalJSONEnum(TeamType_value, data, "TeamType")
	if err != nil {
		return err
	}
	*x = TeamType(value)
	return nil
}
func (TeamType) EnumDescriptor() ([]byte, []int) { return fileDescriptor0, []int{2} }

func init() {
	proto.RegisterEnum("PBCommon.CSID", CSID_name, CSID_value)
	proto.RegisterEnum("PBCommon.SCID", SCID_name, SCID_value)
	proto.RegisterEnum("PBCommon.TeamType", TeamType_name, TeamType_value)
}

func init() { proto.RegisterFile("PBCommon.proto", fileDescriptor0) }

var fileDescriptor0 = []byte{
	// 300 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0x64, 0x8f, 0xc1, 0x4e, 0x2a, 0x41,
	0x10, 0x45, 0xdf, 0x00, 0xef, 0x05, 0xea, 0x45, 0x53, 0xb6, 0x82, 0x62, 0xd4, 0x0f, 0x60, 0x61,
	0x22, 0x68, 0x8c, 0x1b, 0x93, 0xa6, 0xa7, 0x84, 0x49, 0x66, 0xba, 0xdb, 0xee, 0x1a, 0x0d, 0xab,
	0x8e, 0x0b, 0x96, 0x38, 0xc4, 0xb8, 0xf1, 0x83, 0xfc, 0x2a, 0x7f, 0xc6, 0x74, 0x60, 0x88, 0xe0,
	0xee, 0xd6, 0xb9, 0x49, 0xe5, 0x1e, 0xd8, 0xb7, 0x63, 0x55, 0x2d, 0x16, 0xd5, 0xeb, 0xe5, 0xf2,
	0xad, 0x7a, 0xaf, 0x44, 0xbb, 0xbe, 0x07, 0x9f, 0x09, 0xb4, 0x94, 0xcf, 0x52, 0xb1, 0x07, 0x1d,
	0x56, 0x36, 0xe4, 0x66, 0x92, 0x69, 0x4c, 0x44, 0x17, 0x0e, 0xe2, 0xe9, 0xe8, 0xb1, 0x24, 0xcf,
	0xa1, 0x90, 0xac, 0xa6, 0x08, 0xe2, 0x08, 0x30, 0x62, 0x25, 0xb5, 0xa2, 0x7c, 0x4d, 0xff, 0x47,
	0x5a, 0xa6, 0x36, 0x8c, 0x25, 0x73, 0x4e, 0xc1, 0x91, 0x4c, 0x67, 0x38, 0x12, 0x67, 0x70, 0x12,
	0x69, 0x69, 0x83, 0xcd, 0xe5, 0x8c, 0x5c, 0x30, 0x96, 0x9c, 0xe4, 0xcc, 0x68, 0x8f, 0x37, 0xe2,
	0x18, 0x0e, 0xd7, 0x6d, 0x4a, 0x39, 0xcb, 0xf0, 0xe0, 0x64, 0x41, 0x1e, 0x6f, 0xeb, 0x67, 0xa5,
	0x0d, 0x13, 0x59, 0x50, 0x30, 0x4f, 0xe4, 0xf0, 0x6e, 0xf0, 0x95, 0x40, 0xcb, 0xab, 0x2c, 0x15,
	0x3d, 0x10, 0xab, 0x61, 0xde, 0x1a, 0xed, 0x69, 0x33, 0xf8, 0x02, 0x4e, 0xb7, 0xf8, 0xee, 0xf2,
	0x73, 0xe8, 0x6f, 0xf5, 0xbf, 0x15, 0x62, 0x4d, 0x9a, 0xc9, 0xad, 0x45, 0x70, 0xb8, 0x23, 0xe6,
	0x59, 0x3a, 0xc6, 0x51, 0x7c, 0x15, 0x69, 0x6a, 0x9e, 0xf5, 0x6a, 0xf6, 0xb6, 0x59, 0x1f, 0xba,
	0x9b, 0x7a, 0xc7, 0xad, 0x07, 0x62, 0x53, 0xfd, 0xb4, 0xbb, 0x87, 0x36, 0xcf, 0x5f, 0x16, 0xfc,
	0xb1, 0x9c, 0x8b, 0x7f, 0xd0, 0xd0, 0x06, 0xff, 0x88, 0x0e, 0xfc, 0x65, 0x92, 0xc5, 0x15, 0x26,
	0x75, 0x1c, 0x62, 0xa3, 0x8e, 0x23, 0x6c, 0xd6, 0xf1, 0x1a, 0x5b, 0xe3, 0xc6, 0xb4, 0xf9, 0x1d,
	0x00, 0x00, 0xff, 0xff, 0xae, 0x01, 0xb1, 0xf3, 0xe4, 0x01, 0x00, 0x00,
}
