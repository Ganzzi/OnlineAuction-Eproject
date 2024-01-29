import { User } from '@/types/models/user'
import React from 'react'



type Props = {}
const user: User[]=[
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: true ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
  {
    userId: 1,
    username: "Trường",
    email: "abc@gmail.com" ,
    password: "abcxyzzzz" ,
    role: "admin" ,
    refreshToken: "sdfgbhnjm," ,
    locked: false ,
    resetPasswordToken: 'ưerftgyhj' ,
    bids:[],
    soldItems: [] ,
  },
]

const UserList = (props: Props) => {

  return (
    <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
      <div className="py-6 px-4 md:px-6 xl:px-7.5">
        <h4 className="text-xl font-semibold text-black dark:text-white">
          User
        </h4>
      </div>

      <div className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5">
        <div className="col-span-1 flex items-center">
          <p className="font-medium">userId</p>
        </div>
        <div className="col-span-1  items-center">
          <p className="font-medium">username</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">email</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">password</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">role</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">refreshToken</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">locked</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Action</p>
        </div>
      </div>
    
      {user.map((user, key) => (
        <div
          className="grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5"
          key={key}
        >
          <div className="col-span-1 flex items-center">
            <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
            
                 { user.userId}
       
            </div>
          </div>
          <div className="col-span-1  items-center sm:flex">
            <p className="text-sm text-black dark:text-white">
              {user.username}
            </p>
          </div>
          <div className="col-span-1  items-center sm:flex">
            <p className="text-sm text-black dark:text-white">
              {user.email}
            </p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-black dark:text-white">
              {user.password}
            </p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-black dark:text-white">{user.role}</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-meta-3">{user.refreshToken}</p>
          </div>
          <div className="col-span-1 flex items-center">
            <p className="text-sm text-meta-3">{JSON.stringify(user.locked)}</p>
          </div>
          <div className="col-span-1 flex items-center">
           {user.locked ? (
            <div>  <button className='inline-flex items-center justify-center rounded-md bg-primary py-4 px-10 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5'>Lock</button></div>
           ) : (
            <div>
              <button className='inline-flex items-center justify-center rounded-md bg-meta-3 py-4 px-10 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5'>Unlock</button>
            </div>
           )}
          </div>
        </div>
      ))}
    </div>
  );

  
}

export default UserList