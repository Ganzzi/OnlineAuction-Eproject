import { UserData } from '@/app/(admin)/dashboard/users/page';
import axiosService from '@/services/axiosService';
import { User } from '@/types/models/user'
import React, { useState } from 'react'

const UserList = ({ userData }: { userData: UserData[] }) => {

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
          <p className="font-medium">Avg Rate</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Ratings</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Bids</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">role</p>
        </div>
        <div className="col-span-1 flex items-center">
          <p className="font-medium">Action</p>
        </div>
      </div>

      {userData.map((user, key) => (
        <UserCard user={user} key={key}/>
      ))}
    </div>
  );
}

const UserCard = ({ user }: { user: UserData }) => {
  const [isLocked, setIsLocked] = useState(user.user.role == "Disable")

  const handleButtonClick = async (userId: number) => {
    await axiosService.post(`/api/admin/LockUnlockUser/${userId}`).then(() => {
      setIsLocked(!isLocked);
    });

  };

  return (
    <div
    className="hover:bg-meta-3 grid grid-cols-6 border-t border-stroke py-4.5 px-4 dark:border-strokedark sm:grid-cols-8 md:px-6 2xl:px-7.5">
      <div className="col-span-1 flex items-center">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-center">
          {user.user.userId}
        </div>
      </div>
      <div className="col-span-1 items-center sm:flex">
        <p className="text-sm text-black dark:text-white">{user.user.name}</p>
      </div>
      <div className="col-span-1 items-center sm:flex">
        <p className="text-sm text-black dark:text-white">{user.user.email}</p>
      </div>
      <div className="col-span-1 items-center sm:flex">
        <p className="text-sm text-black dark:text-white">{user.avgRate == -1 ? "N/A" : user.avgRate}</p>
      </div>
      <div className="col-span-1 items-center sm:flex">
        <p className="text-sm text-black dark:text-white">{user.ratings}</p>
      </div>
      <div className="col-span-1 items-center sm:flex">
        <p className="text-sm text-black dark:text-white">{user.bidCount}</p>
      </div>
      <div className="col-span-1 flex items-center">
        <p className="text-sm text-black dark:text-white">{user.user.role}</p>
      </div>
      <div className="col-span-1 flex items-center">
        {user.user.role !== "Admin" && (
          <button
            className={`inline-flex items-center justify-center rounded-md ${isLocked ? 'bg-primary' : 'bg-meta-3'} py-4 px-10 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5`}
            onClick={() => handleButtonClick(user.user.userId)}
          >
            {isLocked ? 'Unlock' : 'Lock'}
          </button>
        )}
      </div>
    </div>
  )
}

export default UserList