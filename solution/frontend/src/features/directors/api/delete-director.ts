import { useMutation, useQueryClient } from '@tanstack/react-query'

import { api } from '@/lib/api-client'
import { MutationConfig } from '@/lib/react-query'

import { getDirectorsQueryOptions } from './get-directors'

export const deleteDirector = ({ uniqueId }: { uniqueId: string }) => {
  return api.delete(`/directors/${uniqueId}`)
}

type UseDeleteDirectorOptions = {
  mutationConfig?: MutationConfig<typeof deleteDirector>
}

export const useDeleteDirector = ({ mutationConfig }: UseDeleteDirectorOptions = {}) => {
  const queryClient = useQueryClient()

  const { onSuccess, ...restConfig } = mutationConfig || {}

  return useMutation({
    onSuccess: (...args) => {
      queryClient.invalidateQueries({
        queryKey: getDirectorsQueryOptions().queryKey
      })
      onSuccess?.(...args)
    },
    ...restConfig,
    mutationFn: deleteDirector
  })
}
