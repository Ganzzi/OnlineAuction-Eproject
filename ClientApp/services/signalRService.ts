import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

type SignalRService = {
    hubConnection: HubConnection | null;
    onAuctionEnded: (callback: (itemId: number, sellerId: number) => void) => void;
    offAuctionEnded: (callback: (itemId: number, sellerId: number) => void) => void;
    onSomeoneJoinItemRoom: (callback: (itemId: number, userId: number, bidAmount: number) => void) => void;
    offSomeoneJoinItemRoom: (callback: (itemId: number, userId: number, bidAmount: number) => void) => void;
  
    startConnection: (userId: number) => Promise<void>;
    closeConnection: (userId: number) => Promise<void>;
    joinItemRoom: (userId: number, itemId: number, bidAmount: number) => Promise<void>;
  };

const signalRService: SignalRService = {
  hubConnection: null,
  onAuctionEnded: (callback: (itemId: number, sellerId: number) => void) => {
    if (signalRService.hubConnection && signalRService.hubConnection.state === 'Connected') {
      signalRService.hubConnection.on('AuctionEnded', callback);
    }
  },
  offAuctionEnded: (callback: (itemId: number, sellerId: number) => void) => {
    if (signalRService.hubConnection && signalRService.hubConnection.state === 'Connected') {
      signalRService.hubConnection.off('AuctionEnded', callback);
    }
  },
  onSomeoneJoinItemRoom: (callback: (itemId: number, userId: number, bidAmount: number) => void) => {
    if (signalRService.hubConnection && signalRService.hubConnection.state === 'Connected') { 
      signalRService.hubConnection.on('someonejoinitemroom', callback);
    }
  },
  offSomeoneJoinItemRoom: (callback: (itemId: number, userId: number, bidAmount: number) => void) => {
    if (signalRService.hubConnection && signalRService.hubConnection.state === 'Connected') {
        signalRService.hubConnection.off('someonejoinitemroom', callback);
    }
},

  startConnection: async (userId) => {
    signalRService.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7073/auctionHub')
      .configureLogging(LogLevel.Information)
      .build();

    await signalRService.hubConnection.start();

    console.log('SignalR Connected');

    await signalRService.hubConnection.invoke('OnUserConnected', userId);
  },

  closeConnection: async (userId) => {
    if (signalRService.hubConnection && signalRService.hubConnection.state === 'Connected') {
      await signalRService.hubConnection.invoke('OnUserDisconnected', userId);
      console.log('SignalR Disconnected');
      await signalRService.hubConnection.stop();
    }
  },

  joinItemRoom: async (userId, itemId, bidAmount) => {
    if (signalRService.hubConnection && signalRService.hubConnection.state === 'Connected') {
      console.log("invoking...");
      
      await (signalRService.hubConnection as HubConnection).invoke('JoinItemRoom', {
        userId,
        itemId,
        bidAmount,
      });
    }
  },
};

export default signalRService;
