'use client'

import useColorMode from '@/hooks/useColorMode';
import useLocalStorage from '@/hooks/useLocalStorage';
import { Notification } from '@/types/models/notification';
import { User } from '@/types/models/user';
import { ReactNode, createContext, useContext, useEffect, useState } from 'react';
import { auctionHistory1, auctionHistory2, bid1, bid2, item1, item2, user1 } from '@/data/item';
import axiosService from '@/services/axiosService';
import signalRService from '@/services/signalRService';

type GlobalStateProp = {
  user: User,
  accessToken: string,
  setUser: (user: User) => void,
  setAccessToken: (
    tk: string | null,
    refreshToken?: string) => void,
  notifications: Notification[],
  colorMode: string,
  setColorMode: (color: string) => void,
  isLoggedIn: boolean
}

const initState: GlobalStateProp = {
  user: {
    ...user1,
    soldItems: [
      item1,
      item2
    ], bids: [
      bid1, bid2
    ], auctionHistories: [
      auctionHistory1, auctionHistory2
    ]
  },
  accessToken: '',
  setUser: function (user: User): void {
    throw new Error('Function not implemented.');
  },
  setAccessToken: function (tk: string | null, refreshToken?: string): void {
    throw new Error('Function not implemented.');
  },
  notifications: [],
  setColorMode: function (tk: string | null): void {
    throw new Error('Function not implemented.');
  },
  colorMode: '',
  isLoggedIn: false
}
export const GlobalState = createContext<GlobalStateProp>(initState);

export const GlobalStateProvider = ({ children }: { children: ReactNode }) => {
  const [getColorMode, setColorModeStorage] = useColorMode();
  const [getToken, setToken, removeToken] = useLocalStorage("ACCESS_TOKEN", "");
  const [_g3, setRefreshToken, removeRefreshToken] = useLocalStorage("REFRESH_TOKEN", "");

  const [user, setUser] = useState<User>(initState.user);
  const [accessToken, _setAccessToken] = useState<string>(getToken)
  const [notifications, setNotifications] = useState<Array<Notification>>(initState.notifications)
  const [colorMode, _setColorMode] = useState(getColorMode)
  const [isLoggedIn, setIsLoggedIn] = useState(getToken !== '');

  const setAccessToken: (tk: string | null, refreshToken?: string) => void = (tk: string | null, refreshToken?: string) => {
    if (tk != null && refreshToken) {
      setToken(tk);
      setRefreshToken(refreshToken);
      _setAccessToken(tk)
      setIsLoggedIn(!isLoggedIn)
    } else {
      removeToken()
      removeRefreshToken()
      _setAccessToken("")
      setIsLoggedIn(!isLoggedIn)
    }
  }

  const setColorMode = (colorMode: string) => {
    if (typeof setColorModeStorage === 'function') {
      setColorModeStorage(colorMode);
    }
    _setColorMode(colorMode);
  }

  // use effect to get notifications
  useEffect(() => {
    const fetchUserInfo = async () => {
      const res = await axiosService.get("/api/user/Profile");
      const user: User = res.data.user;

      await signalRService.startConnection(user.userId);

      setUser(user);
    }

    if (accessToken !== "") {
      fetchUserInfo();
    }

    return () => {
      if (accessToken !== "" && user.userId != 0) {
        signalRService.closeConnection(user.userId);
      }
    }
  }, [accessToken])

  useEffect(() => {
    const handleSomeoneJoinItemRoom = (itemId: number, itemTitle: string, userId: number, username: string, bidAmount: number) => {
      const newNotification: Notification = {
        notificationContent: `${userId == user.userId ? "you" : username} has placed a new bid on ${itemTitle} amount ${bidAmount}`,
        notificationDate: new Date().toDateString(),
        notificationId: -1,
        userId,
        itemId
      };
    
      // Check if the notification already exists in the array
      const isNotificationExists = user.notifications?.some(
        (notification) => notification.notificationId === -1
      );
  
      // Update the user state only if the notification doesn't exist
      if (!isNotificationExists) {
        setUser((prevUser) => ({
          ...prevUser,
          notifications: [newNotification,...(prevUser.notifications || [])], // Use non-null assertion operator (!)
        }));
      }
    };
  
    const handleAuctionEnded = (itemId: number, itemTitle: string, sellerId: number, winnerId: number) => {
      const newNotification: Notification = {
        notificationContent: `${itemTitle} has end. ${user.userId == winnerId && "You are the winner"}`,
        notificationDate: new Date().toDateString(),
        notificationId: -2,
        userId: user.userId,
        itemId
      };
    
      // Check if the notification already exists in the array
      const isNotificationExists = user.notifications?.some(
        (notification) => notification.notificationId === -2
      );
  
      // Update the user state only if the notification doesn't exist
      if (!isNotificationExists) {
        setUser((prevUser) => ({
          ...prevUser,
          notifications: [newNotification, ...(prevUser.notifications || [])], // Use non-null assertion operator (!)
        }));
      }
    };
  
    // Subscribe to the event when the component mounts
    signalRService.onSomeoneJoinItemRoom(handleSomeoneJoinItemRoom);
    signalRService.onAuctionEnded(handleAuctionEnded);
  
    // // Unsubscribe when the component unmounts
    return () => {
      signalRService.offSomeoneJoinItemRoom(handleSomeoneJoinItemRoom);
      signalRService.offAuctionEnded(handleAuctionEnded);
    };
  }); // Add user.notifications to the dependency array
  

  return (
    <GlobalState.Provider value={{
      isLoggedIn,
      user,
      setUser,
      accessToken,
      setAccessToken,
      notifications,
      colorMode,
      setColorMode
    }}>
      {children}
    </GlobalState.Provider>
  );
};

export const useGlobalState: () => GlobalStateProp = () => {
  const {
    isLoggedIn,
    user,
    setUser,
    accessToken,
    setAccessToken,
    notifications,
    colorMode, setColorMode
  } = useContext(GlobalState);

  return {
    user,
    setUser,
    accessToken,
    setAccessToken,
    notifications,
    colorMode, setColorMode,
    isLoggedIn
  };
};