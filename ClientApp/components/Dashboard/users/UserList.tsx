import { User } from '@/types/models/user'
import React from 'react'



type Props = {}
const user: User[]=[
  {
    userId: 1,
    username: 'sjhbcj',
    email:"sdbfvjshbd" ,
    password: "jsbdckjsbd",
    role: "jdbsjshbd",
    refreshToken:"jdbsjshbd",
    locked: true,
  },
]
  

const UserList = (props: Props) => {
  return (
    <div>UserList</div>
  )
}

export default UserList