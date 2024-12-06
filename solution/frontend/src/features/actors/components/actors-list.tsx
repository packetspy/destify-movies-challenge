import { useQueryClient } from '@tanstack/react-query'
import { useSearchParams } from 'react-router-dom'
import { useState } from 'react'

import { Link } from '@/components/ui/link'
import { Spinner } from '@/components/ui/spinner'
import { Table } from '@/components/ui/table'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'

import { paths } from '@/config/paths'
import { getActorQueryOptions } from '../api/get-actor'
import { useActors } from '../api/get-actors'
import { DeleteActor } from './delete-actor'
import { SearchActors } from './actors-search'
import { Eye } from 'lucide-react'

export type ActorsListProps = {
  onActorPrefetch?: (id: string) => void
}

export const ActorsList = ({ onActorPrefetch }: ActorsListProps) => {
  const [searchParams] = useSearchParams()
  const [searchQuery, setSearchQuery] = useState('')

  const ActorsQuery = useActors({
    query: searchQuery,
    page: +(searchParams.get('page') || 1)
  })

  const queryClient = useQueryClient()

  if (ActorsQuery.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    )
  }

  const Actors = ActorsQuery.data?.data
  const meta = ActorsQuery.data?.meta

  if (!Actors) return null

  const handleSearch = (query: string) => {
    if (query !== '' || query != undefined) setSearchQuery(query)
  }

  return (
    <>
      <SearchActors onSearch={handleSearch} />
      <Separator className="my-4" />
      <Table
        data={Actors}
        columns={[
          {
            title: 'Name',
            field: 'name',
            Cell({ entry: { name } }) {
              return <div className="w-96">{name}</div>
            }
          },
          {
            title: 'Actions',
            field: 'uniqueId',
            Cell({ entry: { uniqueId } }) {
              return (
                <>
                  <Link
                    className="mr-2"
                    onMouseEnter={() => {
                      queryClient.prefetchQuery(getActorQueryOptions(uniqueId))
                      onActorPrefetch?.(uniqueId)
                    }}
                    to={paths.app.actor.getHref(uniqueId)}
                  >
                    <Button>
                      <Eye className="size-4" />
                    </Button>
                  </Link>

                  <DeleteActor ActorUniqueId={uniqueId} />
                </>
              )
            }
          }
        ]}
        pagination={
          meta && {
            totalPages: meta.totalPages,
            currentPage: meta.page,
            rootUrl: ''
          }
        }
      />
    </>
  )
}
