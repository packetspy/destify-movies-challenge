import { nanoid } from 'nanoid'
import { create } from 'zustand'

export type Notification = {
  id: string
  type: 'info' | 'warning' | 'success' | 'error'
  title: string
  message?: string
  delay?: number
}

type NotificationsStore = {
  notifications: Notification[]
  addNotification: (notification: Omit<Notification, 'id'>) => void
  dismissNotification: (id: string) => void
}

export const useNotifications = create<NotificationsStore>((set) => ({
  notifications: [],
  addNotification: (notification) => {
    const id = nanoid()
    let { delay = 4000 } = notification
    if (delay < 0) delay = 4000

    set((state) => ({
      notifications: [...state.notifications, { id, ...notification }]
    }))

    if (delay && Number.isFinite(delay)) {
      setTimeout(() => {
        set((state) => ({
          notifications: state.notifications.filter((n) => n.id !== id)
        }))
      }, delay)
    }
  },
  dismissNotification: (id) =>
    set((state) => ({
      notifications: state.notifications.filter((notification) => notification.id !== id)
    }))
}))
